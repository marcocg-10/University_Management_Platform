export const minimap = {
    canvas: null,
    ctx: null,
    dotnet: null,
    objects: [],
    dragging: false,
    draggingObject: null,
    dragStartObjectPos: null,
    hoveredObject: null,
    mouseX: 0,
    mouseY: 0,
    zoom: 1,
    offsetX: 0,
    offsetY: 0,

    // Grid settings
    showGrid: true,
    gridSize: 20,
    gridColor: "#e0e0e0",
    axisColor: "#888888",
    baseGridSize: 10,

    // Controls panel settings
    showControlsPanel: true,
    controlsPanelWidth: 120,
    controlsPanelHeight: 60,
    controlsPanelX: 10,
    controlsPanelY: 40,

    init(canvas, dotnetRef) {
        this.canvas = canvas;
        this.ctx = canvas.getContext("2d");
        this.dotnet = dotnetRef;

        // Mouse Events
        canvas.addEventListener("mousedown", e => this.startDrag(e));
        canvas.addEventListener("mousemove", e => this.drag(e));
        canvas.addEventListener("mouseup", e => this.stopDrag(e));
        canvas.addEventListener("mouseleave", () => this.stopDrag());

        // Zoom
        canvas.addEventListener("wheel", e => this.onWheel(e));

        requestAnimationFrame(() => this.loop());
    },

    // --- Public API ---
    addObject(id, x, y, color = "red", width = 10, length = 10, draggable = false, rotationDeg = 0) {
        this.objects.push({ id, x, y, color, width, length, draggable, rotationDeg });
    },

    moveObject(id, x, y) {
        let obj = this.objects.find(o => o.id === id);
        if (!obj) return;

        obj.x = x;
        obj.y = y;
    },

    updateObjectPosition(id, x, y) {
        let obj = this.objects.find(o => o.id === id);
        if (!obj) return false;

        obj.x = x;
        obj.y = y;
        return true;
    },

    updateObjectRotation(id, rotationDeg) {
        let obj = this.objects.find(o => o.id === id);
        if (!obj) return false;

        obj.rotationDeg = rotationDeg ?? 0;
        return true;
    },

    setZoom(z) { this.zoom = z; },
    setOffset(x, y) { this.offsetX = x; this.offsetY = y; },
    setGridVisibility(visible) { this.showGrid = visible; },
    setGridSize(size) { this.gridSize = size; },
    setGridColors(gridColor, axisColor) {
        this.gridColor = gridColor || this.gridColor;
        this.axisColor = axisColor || this.axisColor;
    },

    setControlsPanelVisibility(visible) { this.showControlsPanel = visible; },
    setControlsPanelPosition(x, y) {
        this.controlsPanelX = x;
        this.controlsPanelY = y;
    },

    toggleGrid() {
        this.showGrid = !this.showGrid;
        this.updateExternalUI(this.calculateDynamicScale().scale);
    },

    setObjectDraggable(id, draggable) {
        let obj = this.objects.find(o => o.id === id);
        if (obj) {
            obj.draggable = draggable;
        }
    },

    getCollisions() {
        const collidingObjects = this.getCollidingObjects();
        return Array.from(collidingObjects);
    },

    getObjectAt(mouseX, mouseY) {
        const worldX = (mouseX - this.offsetX) / this.zoom;
        const worldY = (mouseY - this.offsetY) / this.zoom;

        for (let obj of this.objects) {
            const left = obj.x - obj.width / 2;
            const right = obj.x + obj.width / 2;
            const top = obj.y - obj.length / 2;
            const bottom = obj.y + obj.length / 2;

            if (worldX >= left && worldX <= right && worldY >= top && worldY <= bottom) {
                return obj;
            }
        }
        return null;
    },

    calculateDynamicScale() {
        const baseSpacing = this.baseGridSize;
        const zoom = this.zoom;
        let labelStepLines;
        if (zoom >= 16) labelStepLines = 1;
        else if (zoom >= 8) labelStepLines = 1;
        else if (zoom >= 4) labelStepLines = 2;
        else if (zoom >= 2) labelStepLines = 5;
        else if (zoom >= 1) labelStepLines = 10;
        else if (zoom >= 0.5) labelStepLines = 20;
        else if (zoom >= 0.25) labelStepLines = 50;
        else labelStepLines = 100;

        const gridSpacing = baseSpacing;
        const labelStep = labelStepLines * baseSpacing;
        const scaleIndicator = labelStepLines * baseSpacing;

        return {
            gridSpacing,
            labelStep,
            scale: scaleIndicator
        };
    },

    // --- Collision Detection ---
    objectsCollide(obj1, obj2) {
        if (obj1.id === obj2.id) return false;

        const left1 = obj1.x - obj1.width / 2;
        const right1 = obj1.x + obj1.width / 2;
        const top1 = obj1.y - obj1.length / 2;
        const bottom1 = obj1.y + obj1.length / 2;

        const left2 = obj2.x - obj2.width / 2;
        const right2 = obj2.x + obj2.width / 2;
        const top2 = obj2.y - obj2.length / 2;
        const bottom2 = obj2.y + obj2.length / 2;

        return !(right1 < left2 || left1 > right2 || bottom1 < top2 || top1 > bottom2);
    },

    getCollidingObjects() {
        const collisions = new Set();

        for (let i = 0; i < this.objects.length; i++) {
            for (let j = i + 1; j < this.objects.length; j++) {
                if (this.objectsCollide(this.objects[i], this.objects[j])) {
                    collisions.add(this.objects[i].id);
                    collisions.add(this.objects[j].id);
                }
            }
        }

        return collisions;
    },

    drawGrid() {
        if (!this.showGrid) return;

        const ctx = this.ctx;
        const canvas = this.canvas;

        // Dynamic scale based on current zoom
        const dynamicScale = this.calculateDynamicScale();
        const currentGridSize = dynamicScale.gridSpacing; // world units
        const labelStep = dynamicScale.labelStep;         // world units between labels

        // Draw grid lines in world-space (transformed)
        ctx.save();
        ctx.translate(this.offsetX, this.offsetY);
        ctx.scale(this.zoom, this.zoom);

        // Visible area in world coordinates
        const startX = Math.floor((-this.offsetX / this.zoom) / currentGridSize) * currentGridSize;
        const endX = Math.ceil((canvas.width - this.offsetX) / this.zoom / currentGridSize) * currentGridSize;
        const startY = Math.floor((-this.offsetY / this.zoom) / currentGridSize) * currentGridSize;
        const endY = Math.ceil((canvas.height - this.offsetY) / this.zoom / currentGridSize) * currentGridSize;

        // Draw grid lines
        ctx.strokeStyle = this.gridColor;
        ctx.lineWidth = 1 / this.zoom; // constant screen thickness
        ctx.beginPath();

        // Vertical lines
        for (let x = startX; x <= endX; x += currentGridSize) {
            ctx.moveTo(x, startY - currentGridSize);
            ctx.lineTo(x, endY + currentGridSize);
        }

        // Horizontal lines
        for (let y = startY; y <= endY; y += currentGridSize) {
            ctx.moveTo(startX - currentGridSize, y);
            ctx.lineTo(endX + currentGridSize, y);
        }

        ctx.stroke();

        // Draw main axes (X=0, Y=0) with different color
        ctx.strokeStyle = this.axisColor;
        ctx.lineWidth = 2 / this.zoom;
        ctx.beginPath();

        // X axis (horizontal line at y=0)
        ctx.moveTo(startX - currentGridSize, 0);
        ctx.lineTo(endX + currentGridSize, 0);

        // Y axis (vertical line at x=0)
        ctx.moveTo(0, startY - currentGridSize);
        ctx.lineTo(0, endY + currentGridSize);

        ctx.stroke();
        ctx.restore();

        // Draw coordinate labels in screen-space
        const toScreenX = (wx) => wx * this.zoom + this.offsetX;
        const toScreenY = (wy) => wy * this.zoom + this.offsetY;

        const worldStartX = Math.floor((-this.offsetX / this.zoom) / labelStep) * labelStep;
        const worldEndX = Math.ceil((canvas.width - this.offsetX) / this.zoom / labelStep) * labelStep;
        const worldStartY = Math.floor((-this.offsetY / this.zoom) / labelStep) * labelStep;
        const worldEndY = Math.ceil((canvas.height - this.offsetY) / this.zoom / labelStep) * labelStep;

        const fontSize = Math.max(11, Math.min(16, 12 + Math.log2(this.zoom + 1)));
        ctx.fillStyle = this.axisColor;
        ctx.font = `${fontSize}px Arial`;
        ctx.textBaseline = "middle";

        ctx.textAlign = "center";
        for (let x = worldStartX; x <= worldEndX; x += labelStep) {
            if (x !== 0) {
                const sx = toScreenX(x);
                const sy = toScreenY(0) - 12;
                if (sx >= -50 && sx <= canvas.width + 50 && sy >= -20 && sy <= canvas.height + 20) {
                    ctx.fillText(x.toString(), sx, sy);
                }
            }
        }

        ctx.textAlign = "left";
        for (let y = worldStartY; y <= worldEndY; y += labelStep) {
            if (y !== 0) {
                const sx = toScreenX(0) + 6;
                const sy = toScreenY(y);
                if (sx >= -50 && sx <= canvas.width + 50 && sy >= -20 && sy <= canvas.height + 20) {
                    ctx.fillText(y.toString(), sx, sy);
                }
            }
        }

        const originX = toScreenX(0) + 6;
        const originY = toScreenY(0) - 12;
        ctx.textAlign = "left";
        if (originX >= -50 && originX <= canvas.width + 50 && originY >= -20 && originY <= canvas.height + 20) {
            ctx.fillText("(0,0)", originX, originY);
        }

        this.updateExternalUI(dynamicScale.scale);
    },

    updateExternalUI(currentScale) {
        if (this.dotnet) {
            this.dotnet.invokeMethodAsync("UpdateScaleDisplay", currentScale);
            this.dotnet.invokeMethodAsync("UpdateGridToggle", this.showGrid);
        }
    },

    drawScaleIndicator(currentScale) {
        const ctx = this.ctx;
        const canvas = this.canvas;

        ctx.fillStyle = "rgba(255, 255, 255, 0.9)";
        ctx.fillRect(10, 10, 80, 25);

        ctx.strokeStyle = "rgba(0, 0, 0, 0.5)";
        ctx.lineWidth = 1;
        ctx.strokeRect(10, 10, 80, 25);

        ctx.fillStyle = "black";
        ctx.font = "12px Arial";
        ctx.textAlign = "left";
        ctx.textBaseline = "middle";
        ctx.fillText(`Scale 1:${currentScale}`, 15, 22.5);
    },

    drawControlsPanel() {
        if (!this.showControlsPanel) return;

        const ctx = this.ctx;
        const panelX = this.controlsPanelX;
        const panelY = this.controlsPanelY;
        const panelWidth = this.controlsPanelWidth;
        const panelHeight = this.controlsPanelHeight;

        ctx.fillStyle = "rgba(255, 255, 255, 0.95)";
        ctx.fillRect(panelX, panelY, panelWidth, panelHeight);

        ctx.strokeStyle = "rgba(0, 0, 0, 0.3)";
        ctx.lineWidth = 1;
        ctx.strokeRect(panelX, panelY, panelWidth, panelHeight);

        ctx.fillStyle = "black";
        ctx.font = "12px Arial";
        ctx.textAlign = "left";
        ctx.textBaseline = "middle";
        ctx.fillText("Controls", panelX + 8, panelY + 15);

        const buttonX = panelX + 8;
        const buttonY = panelY + 25;
        const buttonWidth = 80;
        const buttonHeight = 25;

        ctx.fillStyle = this.showGrid ? "rgba(0, 150, 0, 0.2)" : "rgba(150, 0, 0, 0.2)";
        ctx.fillRect(buttonX, buttonY, buttonWidth, buttonHeight);

        ctx.strokeStyle = this.showGrid ? "rgba(0, 150, 0, 0.6)" : "rgba(150, 0, 0, 0.6)";
        ctx.lineWidth = 1;
        ctx.strokeRect(buttonX, buttonY, buttonWidth, buttonHeight);

        ctx.fillStyle = "black";
        ctx.font = "11px Arial";
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillText(this.showGrid ? "Grid: ON" : "Grid: OFF", buttonX + buttonWidth / 2, buttonY + buttonHeight / 2);
    },

    // --- Input ---
    startDrag(e) {
        const mouseX = e.offsetX;
        const mouseY = e.offsetY;

        const clickedObject = this.getObjectAt(mouseX, mouseY);

        if (clickedObject && clickedObject.draggable) {
            this.draggingObject = clickedObject;
            this.dragStartObjectPos = { x: clickedObject.x, y: clickedObject.y };
            this.canvas.style.cursor = "move";
        } else {
            this.dragging = true;
            this.canvas.style.cursor = "grabbing";
        }

        this.lastX = mouseX;
        this.lastY = mouseY;
    },

    drag(e) {
        this.mouseX = e.offsetX;
        this.mouseY = e.offsetY;

        if (this.draggingObject) {
            const dx = e.offsetX - this.lastX;
            const dy = e.offsetY - this.lastY;

            const worldDx = dx / this.zoom;
            const worldDy = dy / this.zoom;

            this.draggingObject.x += worldDx;
            this.draggingObject.y += worldDy;

            this.lastX = e.offsetX;
            this.lastY = e.offsetY;
            return;
        }

        if (this.dragging) {
            const dx = e.offsetX - this.lastX;
            const dy = e.offsetY - this.lastY;

            this.offsetX += dx;
            this.offsetY += dy;

            this.lastX = e.offsetX;
            this.lastY = e.offsetY;
            return;
        }

        this.hoveredObject = this.getObjectAt(this.mouseX, this.mouseY);

        if (this.hoveredObject) {
            this.canvas.style.cursor = this.hoveredObject.draggable ? "pointer" : "default";
        } else {
            this.canvas.style.cursor = "grab";
        }
    },

    stopDrag(e) {
        if (this.draggingObject) {
            const obj = this.draggingObject;
            if (this.dotnet) {
                this.dotnet.invokeMethodAsync("NotifyPositionChanged", obj.id, obj.x, obj.y);
            }
            this.draggingObject = null;
            this.dragStartObjectPos = null;
        }

        this.dragging = false;

        const currentHovered = this.getObjectAt(this.mouseX, this.mouseY);
        if (currentHovered) {
            this.canvas.style.cursor = currentHovered.draggable ? "pointer" : "default";
        } else {
            this.canvas.style.cursor = "grab";
        }
    },

    handleClick(e) {
        const mouseX = e.offsetX;
        const mouseY = e.offsetY;

        if (this.isClickOnControlsPanel(mouseX, mouseY)) {
            this.handleControlsPanelClick(mouseX, mouseY);
        }
    },

    isClickOnControlsPanel(mouseX, mouseY) {
        if (!this.showControlsPanel) return false;

        return mouseX >= this.controlsPanelX &&
            mouseX <= this.controlsPanelX + this.controlsPanelWidth &&
            mouseY >= this.controlsPanelY &&
            mouseY <= this.controlsPanelY + this.controlsPanelHeight;
    },

    handleControlsPanelClick(mouseX, mouseY) {
        const buttonX = this.controlsPanelX + 8;
        const buttonY = this.controlsPanelY + 25;
        const buttonWidth = 80;
        const buttonHeight = 25;

        if (mouseX >= buttonX && mouseX <= buttonX + buttonWidth &&
            mouseY >= buttonY && mouseY <= buttonY + buttonHeight) {
            this.showGrid = !this.showGrid;
        }
    },

    onWheel(e) {
        e.preventDefault();

        const zoomStep = 0.5;
        const minZoom = 0.05;
        const maxZoom = 32;

        const delta = e.deltaY > 0 ? -zoomStep : zoomStep;

        const mouseX = e.offsetX;
        const mouseY = e.offsetY;

        const worldX = (mouseX - this.offsetX) / this.zoom;
        const worldY = (mouseY - this.offsetY) / this.zoom;

        const newZoom = Math.max(minZoom, Math.min(maxZoom, this.zoom + delta));

        this.offsetX = mouseX - worldX * newZoom;
        this.offsetY = mouseY - worldY * newZoom;

        this.zoom = newZoom;
    },

    clearObjects() {
        this.objects = [];
    },

    updateObjectScale(id, width, length) {
        let obj = this.objects.find(o => o.id === id);
        if (!obj) return false;

        obj.width = width;
        obj.length = length;
        return true;
    },

    updateObjectColor(id, color) {
        let obj = this.objects.find(o => o.id === id);
        if (!obj) return false;

        obj.color = color;
        return true;
    },

    updateObjectId(oldId, newId) {
        let obj = this.objects.find(o => o.id === oldId);
        if (!obj) return false;

        obj.id = newId;

        if (this.hoveredObject === obj) {
            this.hoveredObject = obj;
        }

        return true;
    },

    // Learning space property to draw boundaries
    learningSpace: null,

    setLearningSpace(width, depth, centerX = 0, centerY = 0) {
        this.learningSpace = {
            width: width,
            depth: depth,
            centerX: centerX,
            centerY: centerY
        };
        console.log('Learning space set:', this.learningSpace);
    },

    clearLearningSpace() {
        this.learningSpace = null;
        console.log('Learning space cleared');
    },

    drawLearningSpaceBoundary() {
        if (!this.learningSpace) {
            console.log('No learning space to draw');
            return;
        }

        const ctx = this.ctx;
        ctx.save();
        ctx.translate(this.offsetX, this.offsetY);
        ctx.scale(this.zoom, this.zoom);

        const halfWidth = this.learningSpace.width / 2;
        const halfDepth = this.learningSpace.depth / 2;
        const left = this.learningSpace.centerX - halfWidth;
        const top = this.learningSpace.centerY - halfDepth;

        console.log(`Drawing boundary at: left=${left}, top=${top}, width=${this.learningSpace.width}, depth=${this.learningSpace.depth}`);
        console.log(`Current zoom: ${this.zoom}, offsetX: ${this.offsetX}, offsetY: ${this.offsetY}`);

        ctx.strokeStyle = 'rgba(0, 100, 200, 0.8)';
        ctx.lineWidth = 2.5 / this.zoom;
        ctx.setLineDash([8 / this.zoom, 4 / this.zoom]);
        ctx.strokeRect(left, top, this.learningSpace.width, this.learningSpace.depth);
        ctx.setLineDash([]);

        ctx.restore();
    },

    // Helper method to check if object is outside learning space (using AABB approximation)
    isObjectOutsideLearningSpace(obj) {
        if (!this.learningSpace) return false;

        const halfWidth = this.learningSpace.width / 2;
        const halfDepth = this.learningSpace.depth / 2;
        const left = this.learningSpace.centerX - halfWidth;
        const right = this.learningSpace.centerX + halfWidth;
        const top = this.learningSpace.centerY - halfDepth;
        const bottom = this.learningSpace.centerY + halfDepth;

        // For rotated objects, we'll use a simple AABB approximation
        // This calculates the axis-aligned bounding box of the rotated rectangle
        const angleRad = ((obj.rotationDeg ?? 0) * Math.PI) / 180;
        const cosA = Math.cos(angleRad);
        const sinA = Math.sin(angleRad);

        const hw = obj.width / 2;
        const hl = obj.length / 2;

        // Calculate the four corners of the rotated rectangle
        const corners = [
            { x: hw * cosA - hl * sinA, y: hw * sinA + hl * cosA },    // top-right
            { x: -hw * cosA - hl * sinA, y: -hw * sinA + hl * cosA },  // top-left
            { x: -hw * cosA + hl * sinA, y: -hw * sinA - hl * cosA },  // bottom-left
            { x: hw * cosA + hl * sinA, y: hw * sinA - hl * cosA }     // bottom-right
        ];

        // Find the min/max bounds of the rotated rectangle
        let minX = Infinity, maxX = -Infinity;
        let minY = Infinity, maxY = -Infinity;

        for (const corner of corners) {
            const worldX = obj.x + corner.x;
            const worldY = obj.y + corner.y;
            minX = Math.min(minX, worldX);
            maxX = Math.max(maxX, worldX);
            minY = Math.min(minY, worldY);
            maxY = Math.max(maxY, worldY);
        }

        // Check if the AABB is outside learning space boundaries
        return minX < left || maxX > right || minY < top || maxY > bottom;
    },

    // Get all objects outside learning space
    getObjectsOutsideLearningSpace() {
        if (!this.learningSpace) return new Set();

        const outsideObjects = new Set();
        for (let obj of this.objects) {
            if (this.isObjectOutsideLearningSpace(obj)) {
                outsideObjects.add(obj.id);
            }
        }
        return outsideObjects;
    },

    isObjectOutsideLearningSpaceAPI(objectId) {
        const obj = this.objects.find(o => o.id === objectId);
        if (!obj) return false;
        return this.isObjectOutsideLearningSpace(obj);
    },

    getObjectsOutsideLearningSpaceAPI() {
        const outsideIds = [];
        for (let obj of this.objects) {
            if (this.isObjectOutsideLearningSpace(obj)) {
                outsideIds.push(obj.id);
            }
        }
        return outsideIds;
    },

    // --- Loop ---
    loop() {
        const ctx = this.ctx;
        const c = this.canvas;
        ctx.clearRect(0, 0, c.width, c.height);

        // Grid
        this.drawGrid();

        // Learning space boundaries
        this.drawLearningSpaceBoundary();

        ctx.save();
        ctx.translate(this.offsetX, this.offsetY);
        ctx.scale(this.zoom, this.zoom);

        // Collisions and out-of-bounds detection
        const collidingObjects = this.getCollidingObjects();
        const outsideObjects = this.getObjectsOutsideLearningSpace();

        if (this.dotnet && collidingObjects.size > 0) {
            this.dotnet.invokeMethodAsync("NotifyCollision", Array.from(collidingObjects));
        }
        else if (this.dotnet && collidingObjects.size === 0) {
            this.dotnet.invokeMethodAsync("NotifyCollision", []);
        }

        // Draw all objects (rotated around center)
        for (let obj of this.objects) {
            const angleRad = ((obj.rotationDeg ?? 0) * Math.PI) / 180;

            ctx.save();
            ctx.translate(obj.x, obj.y);
            ctx.rotate(-angleRad);

            // Body
            ctx.fillStyle = obj.color;
            ctx.fillRect(-obj.width / 2, -obj.length / 2, obj.width, obj.length);

            if (collidingObjects.has(obj.id)) {
                ctx.strokeStyle = "#FF0000";
                ctx.lineWidth = 3 / this.zoom;
                ctx.strokeRect(-obj.width / 2, -obj.length / 2, obj.width, obj.length);
            } else if (outsideObjects.has(obj.id)) {
                ctx.strokeStyle = "#FF0000";
                ctx.lineWidth = 3 / this.zoom;
                ctx.strokeRect(-obj.width / 2, -obj.length / 2, obj.width, obj.length);
            }

            ctx.restore();
        }

        ctx.restore();

        // Hover tooltip
        if (this.hoveredObject) {
            const obj = this.hoveredObject;
            let text = `ID: ${obj.id}, X: ${obj.x.toFixed(1)}, Y: ${obj.y.toFixed(1)}, Rot: ${(obj.rotationDeg ?? 0).toFixed(1)}\u00B0`;

            // Check if this specific object is outside bounds
            if (this.isObjectOutsideLearningSpace(obj)) {
                text += " (Outside of learning space bounds)";
            }

            ctx.fillStyle = "rgba(0, 0, 0, 0.8)";
            ctx.font = "12px Arial";

            const metrics = ctx.measureText(text);
            const textWidth = metrics.width;
            const textHeight = 16;

            let tooltipX = this.mouseX + 10;
            let tooltipY = this.mouseY - 5;

            if (tooltipX + textWidth + 10 > c.width) {
                tooltipX = this.mouseX - textWidth - 10;
            }
            if (tooltipY - textHeight < 0) {
                tooltipY = this.mouseY + 20;
            }

            ctx.fillStyle = "rgba(255, 255, 255, 0.9)";
            ctx.fillRect(tooltipX - 5, tooltipY - textHeight, textWidth + 10, textHeight + 5);

            ctx.strokeStyle = "rgba(0, 0, 0, 0.5)";
            ctx.lineWidth = 1;
            ctx.strokeRect(tooltipX - 5, tooltipY - textHeight, textWidth + 10, textHeight + 5);

            ctx.fillStyle = "black";
            ctx.fillText(text, tooltipX, tooltipY - 2);
        }

        requestAnimationFrame(() => this.loop());
    }
};