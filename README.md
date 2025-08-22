# SVES — Stream Viewers Entertainment System

Stream Viewers Entertainment System (SVES) is a platform for content creators to add interactive activities to their streams and engage viewers in real time.

Designed with Kick integration in mind, SVES brings mini-games, overlays, and a viewer-driven economy into your broadcast.

---

## Features

- Real-time interactivity
  - Low-latency overlays and instant game state updates.
- Kick integration
  - Connect to your Kick channel chat, parse commands, and post results.
- Mini-games
  - Fortune Wheel: configurable segments, weights, and rewards.
  - Tic-Tac-Toe vs Viewer: play using chat commands; queue or direct challenge.
- Viewer economy and filters
  - Viewers can earn/spend points to buy filters/effects on the stream overlay.
- Moderation and safety
  - Cooldowns, anti-spam, role permissions, and banlist for abusive commands.
- Overlay-ready
  - Simple browser source URL for OBS or similar encoders.

---

## Table of Contents

- [Features](#features)
- [Demo / Screenshots](#demo--screenshots)
- [Tech Stack (example)](#tech-stack-example)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Running Locally](#running-locally)
  - [OBS / Overlay Setup](#obs--overlay-setup)
- [Chat Commands](#chat-commands)
- [How It Works](#how-it-works)
- [Game Details](#game-details)
  - [Fortune Wheel](#fortune-wheel)
  - [Tic-Tac-Toe vs Viewer](#tic-tac-toe-vs-viewer)
- [Filters and Purchases](#filters-and-purchases)
- [Security and Moderation](#security-and-moderation)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [FAQ](#faq)
- [Credits](#credits)
- [Notes](#notes)

---

## Demo / Screenshots

- Overlay preview: coming soon
- Fortune Wheel spin: coming soon
- Tic-Tac-Toe playthrough: coming soon

---

## Tech Stack 

- Backend: Blazor .NET with WebSockets (SignalR) for real-time events
- Frontend: Web overlay (Browser Source friendly)
- Database: PostgreSQL or MongoDB
- Streaming software: OBS (or compatible)

---

## Getting Started

### Prerequisites

- .NET 8 or Above
- A database (PostgreSQL or MongoDB)
- OBS (or similar) to add the overlay as a Browser Source
- A Kick account (and bot/service account for chat integration)

### Installation

```bash
# Clone the repository
git clone https://github.com/nieprzecietny/stream-viewers-entertainment-system.git
cd stream-viewers-entertainment-system

# Install dependencies
dotnet restore
dotnet build
```

### Configuration

Copy the example environment and update values:

```bash
cp .env.example .env
```

Example `.env` variables:

```env
APP_URL=http://localhost:3000

# Database
DATABASE_URL=postgres://user:pass@localhost:5432/sves
# or for MongoDB
# DATABASE_URL=mongodb://localhost:27017/sves

# Kick integration
KICK_CHANNEL=yourkickname
KICK_BOT_USERNAME=yourbotname
KICK_CLIENT_ID=optional_if_using_oauth
KICK_CLIENT_SECRET=optional_if_using_oauth
```

### Running Locally

```bash
dotnet run
```

Open the overlay locally (example):
- http://localhost:3000/overlay?channel=yourkickname

### OBS / Overlay Setup

1. In OBS, add a new Browser Source.
2. Set the URL to your overlay, e.g.:
   - Local: `http://localhost:3000/overlay?channel=yourkickname`
   - Production: `https://your-domain.com/overlay?channel=yourkickname`
3. Recommended settings:
   - Size: 1920x1080
   - Enable “Refresh browser when scene becomes active”
   - Enable hardware acceleration (OBS setting)

---

## Chat Commands

All commands are customizable. Examples:

```
coming soon
```

---

## How It Works

- Backend
  - Exposes real-time APIs (e.g., WebSockets) to broadcast game states, spins, and filter triggers to the overlay.
- Frontend overlay
  - Renders games, animations, and filters; subscribes to updates for instant feedback.
- Chat bridge
  - Connects to Kick chat, parses commands, enforces cooldowns/permissions, and forwards events to the backend.
- Economy
  - Stores points, purchases, cooldowns; optionally grants points by watch time or events.

---

## Game Details

### Fortune Wheel
- Configurable segments: label, color, weight/probability, and reward.
- Triggers: chat command, dashboard button, or scheduled spin.
- Anti-spam: per-user and global cooldowns.
- Output: spin animation on overlay, result posted to chat.

### Tic-Tac-Toe vs Viewer
- Matchmaking: viewer queue or direct challenge.
- Turns: chat commands like `!move A1` or `!move 5`.
- Timeouts and auto-forfeit to keep games moving.
- Overlay: live board state, win/draw animations.

---

## Filters and Purchases

- Catalog: define `id`, display name, cost, duration, cooldown, and visual parameters.
- Purchase flow:
  1. Viewer runs `!buy filter_name`.
  2. Bot validates balance, cooldowns, and availability.
  3. Overlay applies the effect (e.g., grayscale, blur, confetti) for the set duration.
- Optional features:
  - Gifting filters to other users.
  - Per-stream caps and refunds on failure.
  - Role-based discounts or subscriber perks.

---

## Security and Moderation

- Per-user command cooldowns and global rate limits.
- Role checks (creator, moderator, subscriber, follower, everyone).
- Overlay auth token to prevent spoofed events.
- Audit logs for spins, purchases, and game outcomes.
- Banlist for abusive users/commands.

---

## Roadmap

- More games: trivia, bingo, connect four.
- Additional platforms: Twitch, YouTube.
- In-stream polls and predictions.
- Web-based store and loyalty tiers.
- Mobile companion for moderators.

---

## Contributing

Contributions are welcome!
1. Fork the repository.
2. Create a feature branch: `git checkout -b feat/your-feature`.
3. Commit changes and open a pull request.
4. Include clear reproduction steps and screenshots for UI changes.

For bugs or feature requests, please open an issue.

---

## License

Reffer to a  `LICENSE` file.

---

## FAQ

- Does this only work with Kick?
  - Initial focus is Kick, but the architecture is platform-agnostic and can extend to others.
- Is there a built-in points system?
  - Yes. Points can be granted by time watched, chat activity, or manual rewards.
- Can I disable features?
  - Yes. Enable/disable games and filters from the dashboard.
- What if chat gets spammy?
  - Use cooldowns, per-user limits, and moderator-only commands.

---

## Credits

- Acknowledge third-party chat libraries, UI assets, fonts, and sound effects used in your implementation.

---

## Notes

- Always comply with Kick’s terms and community guidelines.
- Depending on your approach, you may use community libraries or chat gateways for Kick. Follow rate limits and authentication rules.
