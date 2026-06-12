# **Look & Feel Convention**
> _Ensuring the **user interface (UI)** of our web application is visually appealing, consistent, and user-friendly is crucial for providing a positive **user experience (UX)**.
This document outlines the conventions for the look and feel of our web application, covering aspects such as color schemes, typography, user flow of the application and component styles._

Based on the *“CI-ACP-E01-2016 Standard for Graphical Interface Design in Information Systems”*, this convention establishes mandatory rules for **color usage**, **typography**, **layout**, **accessibility**, and **interaction patterns** to maintain a coherent and user-friendly institutional experience.

---

## **Purpose**
To provide a unified visual and functional identity for all features, ensuring that every interface:

- Reflects the official **UCR image and branding**.
- Promotes **usability, accessibility**, and **clarity**.
- Maintains **consistency** across systems, platforms, and devices.
- Respects the **institutional hierarchy** and official identity policies defined by the **Oficina de Divulgación e Información (ODI)**.

---

## **General Design Principles**

- **Simplicity:** Interfaces should be minimal, intuitive, and free from unnecessary elements.
	> The best interfaces are invisible—allowing users to focus on their tasks.

- **Consistency:** Common patterns in color, layout, and labeling must be maintained across all screens.
	> Users should be able to transfer knowledge from one task or module to another seamlessly.

- **Clarity:** Language, typography, and icons must clearly communicate purpose and functionality.

- **Intentional Layout:** Spatial relationships should emphasize hierarchy and readability.
	> Important elements must stand out through color, contrast, and positioning.

- **Accessibility:** All designs must consider users with disabilities and support navigation via keyboard and assistive technologies.

---

##  **Accessibility Guidelines**

- Text content must not be replaced by images.
- All images must include **alt** and **title** attributes.
- All interactive elements must be accessible via keyboard navigation.
	> Pop-ups, modals, and dropdowns must be operable without a mouse.
- Use **semantic HTML tags** for headings (H1, H2, etc.).
- Uploaded documents must be in accessible formats (`.ODF`, `.DOC`, `.HTML`, or text-based `.PDF`).
- Interfaces must support **color inversion** and **high-contrast modes**.
- Tables are allowed only for **tabular data**, not for positioning. **CSS box models** must be used for layout (not HTML tables).
	> This so screen readers can interpret the data correctly.

---

## **Visual Identity**
All systems must adhere to the **official UCR color palette**, typography, and signature guidelines to preserve the institutional brand.

### **Official Color Palette**
The color palette is divided into primary, secondary, and grayscale colors, which can be shaded at any percentage or opacity.

#### Primary Colors

| Color          | HEX       | RGB             | Usage                                  |
| -------------- | --------- | --------------- | -------------------------------------- |
| UCR Light Blue | `#41ADE7` | (65, 173, 231)  | Base tone for primary bars and accents |
| UCR Blue       | `#204C6F` | (30, 75, 110)   | Used for navigation menus and text     |
| White          | `#FFFFFF` | (255, 255, 255) | Backgrounds and neutral areas          |

#### Neutral Grays
The achromatic grayscale should share spatial predominance on the screen; it is primarily used in backgrounds, lines, and frames that require filling.

| Color        | HEX       | RGB             | Usage                           |
| ------------ | --------- | --------------- | ------------------------------- |
| Light Gray 1 | `#F5F5F5` | (245, 245, 245) | Background for content areas    |
| Light Gray 2 | `#ECECEC` | (236, 236, 236) | Containers and section dividers |
| Gray 3       | `#CCCCCC` | (204, 204, 204) | Borders                         |
| Gray 4       | `#999999` | (153, 153, 153) | Secondary text                  |
| Gray 5       | `#666666` | (102, 102, 102) | Footer and strong contrast text |

#### Secondary Colors

| Color Name     | HEX       | RGB           |
| -------------- | --------- | ------------- |
| Light Blue 2   | `#249DD8` | R35 G160 B215 |
| Bright Blue 1  | `#0090D8` | R0 G145 B215  |
| Bright Blue 2  | `#2980B9` | R40 G130 B185 |
| Dark Blue 1    | `#0C344E` | R10 G50 B80   |
| Green 1        | `#95B60A` | R180 G65 B20  |
| Green 2        | `#609000` | R96 G145 B0   |
| Yellow 1       | `#FFDD00` | R255 G221 B0  |
| Yellow 2       | `#FFCC00` | R255 G205 B0  |
| Orange 1       | `#E46305` | R230 G100 B5  |
| Orange 2       | `#B14212` | R177 G66 B20  |
| Light Orange 1 | `#FDB727` | R255 G180 B40 |
| Light Orange 2 | `#EBA71C` | R235 G165 B30 |

#### Colors to avoid

| Color        | HEX       | RGB            | Usage                                     |
| ------------ | --------- | -------------- | ----------------------------------------- |
| **Red**      | –         | –              | Reserved exclusively for alerts or errors |

---

## **Typography**

The official typefaces for UCR systems are:

| Category          | Typeface        | Use                               |
| ----------------- | --------------- | --------------------------------- |
| Primary           | **Myriad Pro**  | Titles, main headings             |
| Secondary         | **Warnock Pro** | Formal communications             |
| Web-safe fallback | **Arial**       | General use and screen legibility |

> **Arial** is mandatory in digital systems to ensure cross-platform compatibility and optimal readability on screens.

### Hierarchy & Readability

* Use **font weight and size** to establish visual hierarchy.
* Avoid decorative fonts.
* Maintain appropriate **line spacing** and **contrast** for legibility.

---

## **Institutional Signatures**
UCR signatures represent the university’s **ownership and authorship**.

### Types of Signatures

1. **Official Signatures** — Include the UCR shield and the phrase “Universidad de Costa Rica.”
	> Used in formal institutional systems.
2. **Promotional Signatures** — Include “UCR” with or without the phrase “Universidad de Costa Rica
	> Used for public-facing or internal applications.
3. **Typographic Signatures** — Just the phrase “Universidad de Costa Rica” in the official font.
	> Used when the shield cannot be displayed due to space limitations (e.g., headers or navigation bars).

### Size Requirements

* **Minimum width:** 42.52px (1.5 cm).
* **Maximum width:** 130px (to avoid visual overload).


---

## **References**
- [UCR - UI Design Guidelines](https://ci.ucr.ac.cr/sites/default/files/2022-03/CI-C-11-2016%20Publicacion%20de%20Estandar%20para%20dise%C3%B1o%20de%20interfaz%20grafica%20SI.pdf)
	> A comprehensive guide from the University of Costa Rica on UI design principles and standards.

- [Oficina de Divulgación e Información (ODI)](http://odi.ucr.ac.cr)
	> The official office responsible for dissemination and information at the University of Costa Rica, providing resources and guidelines for institutional communication.
