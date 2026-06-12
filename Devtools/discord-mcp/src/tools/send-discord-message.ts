import { z } from "zod";
import { type InferSchema, type ToolMetadata } from "xmcp";
import { config } from "dotenv";

// Load environment variables
config();

// Define the schema for tool parameters
export const schema = {
  message: z.string().describe("The message to send to Discord"),
  username: z.string().optional().describe("Custom username for the webhook (optional)"),
  avatar_url: z.string().url().optional().describe("Custom avatar URL for the webhook (optional)"),
};

// Define tool metadata
export const metadata: ToolMetadata = {
  name: "send-discord-message",
  description: "Send a message to Discord using a webhook",
  annotations: {
    title: "Send Discord Message",
    readOnlyHint: false,
    destructiveHint: false,
    idempotentHint: false,
  },
};

// Tool implementation
export default async function sendDiscordMessage({
  message,
  username,
  avatar_url,
}: InferSchema<typeof schema>) {
  const webhookUrl = process.env.DISCORD_WEBHOOK_URL;
  
  if (!webhookUrl) {
    throw new Error("DISCORD_WEBHOOK_URL environment variable is not set");
  }

  try {
    const payload: any = {
      content: message,
    };

    // Add optional parameters if provided
    if (username) {
      payload.username = username;
    }
    if (avatar_url) {
      payload.avatar_url = avatar_url;
    }

    const response = await fetch(webhookUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Discord webhook failed: ${response.status} ${response.statusText} - ${errorText}`);
    }

    return {
      success: true,
      message: "Message sent successfully to Discord",
      status: response.status,
    };
  } catch (error) {
    return {
      success: false,
      message: `Failed to send message: ${error instanceof Error ? error.message : String(error)}`,
    };
  }
}