# Discord MCP Tool

This xMCP project includes a Discord webhook tool for sending messages to Discord channels.

## Setup

1. **Environment Variables**: The Discord webhook URL is stored in the `.env` file:
   ```
   DISCORD_WEBHOOK_URL=
   ```

2. **Installation**: Make sure all dependencies are installed:
   ```bash
   npm install
   ```

## Usage

### Running the Development Server

```bash
npm run dev
```

The MCP server will be available at `http://127.0.0.1:3001/mcp`

### Discord Tool: `send-discord-message`

Send messages to Discord using the configured webhook.

**Parameters:**
- `message` (required): The message content to send
- `username` (optional): Custom username for the webhook
- `avatar_url` (optional): Custom avatar URL for the webhook

**Example Usage:**

```json
{
  "message": "Hello from xMCP!",
  "username": "Bot Assistant",
  "avatar_url": "https://example.com/avatar.png"
}
```

### Testing the Tool

You can test the Discord webhook tool by connecting any MCP-compatible client to the server and calling the `send-discord-message` tool.

## Building for Production

```bash
npm run build
npm run start
```

## Features

- ✅ Secure webhook URL storage in `.env`
- ✅ Optional custom username and avatar
- ✅ Error handling and status reporting
- ✅ TypeScript support with Zod validation
- ✅ Automatic tool discovery through xMCP

## Project Structure

```
src/
  tools/
    send-discord-message.ts    # Discord webhook tool
  prompts/
    review-code.ts            # Code review prompt
  resources/
    (config)/
      app.ts                  # App configuration resource
    (users)/
      [userId]/
        index.ts              # User profile resource
```