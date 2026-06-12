#!/usr/bin/env node

const https = require('https');
const fs = require('fs');

// Configuration
const REPO = process.env.GITHUB_REPOSITORY || 'UCR-PI-IS/ecci_ci0128_ii2025_g01_pi';
const BRANCH = process.env.BRANCH || 'development';
const TOKEN = process.env.GITHUB_TOKEN;
const TODAY = new Date().toISOString().split('T')[0];

// Calculate 24 hours ago
const twentyFourHoursAgo = new Date(Date.now() - 24 * 60 * 60 * 1000);

if (!TOKEN) {
  console.error('::error::GITHUB_TOKEN is not set');
  process.exit(1);
}

// Helper function to make GitHub API requests
function githubAPI(path, options = {}) {
  return new Promise((resolve, reject) => {
    const url = new URL(`https://api.github.com${path}`);
    
    // Add query parameters
    if (options.params) {
      Object.entries(options.params).forEach(([key, value]) => {
        url.searchParams.append(key, value);
      });
    }

    const requestOptions = {
      headers: {
        'Authorization': `token ${TOKEN}`,
        'User-Agent': 'GitHub-Actions-Daily-Report',
        'Accept': 'application/vnd.github.v3+json'
      }
    };

    https.get(url.toString(), requestOptions, (res) => {
      let data = '';

      res.on('data', (chunk) => {
        data += chunk;
      });

      res.on('end', () => {
        if (res.statusCode >= 200 && res.statusCode < 300) {
          try {
            resolve(JSON.parse(data));
          } catch (e) {
            reject(new Error(`Failed to parse JSON: ${e.message}`));
          }
        } else {
          reject(new Error(`HTTP ${res.statusCode}: ${data}`));
        }
      });
    }).on('error', (err) => {
      reject(err);
    });
  });
}

// Fetch commits
async function fetchCommits() {
  try {
    console.log(`Fetching commits from last 24 hours on ${BRANCH}...`);
    const commits = await githubAPI(`/repos/${REPO}/commits`, {
      params: {
        sha: BRANCH,
        since: twentyFourHoursAgo.toISOString(),
        per_page: 100
      }
    });

    const processedCommits = commits.map(commit => ({
      message: commit.commit.message,
      author: commit.commit.author.name,
      date: commit.commit.author.date,
      url: commit.html_url,
      sha: commit.sha.substring(0, 7)
    }));

    console.log(`✅ Successfully fetched ${processedCommits.length} commits from last 24h`);
    return processedCommits;
  } catch (error) {
    console.log(`::warning::Failed to fetch commits: ${error.message}`);
    return [];
  }
}

// Fetch pull requests
async function fetchPRs() {
  try {
    console.log(`Fetching PRs from last 24 hours targeting ${BRANCH}...`);
    const prs = await githubAPI(`/repos/${REPO}/pulls`, {
      params: {
        base: BRANCH,
        state: 'all',
        per_page: 100,
        sort: 'updated',
        direction: 'desc'
      }
    });

    // Filter PRs updated in the last 24 hours
    const recentPRs = prs.filter(pr => {
      const updatedAt = new Date(pr.updated_at);
      return updatedAt >= twentyFourHoursAgo;
    });

    const processedPRs = recentPRs.map(pr => ({
      number: pr.number,
      title: pr.title,
      author: pr.user.login,
      url: pr.html_url,
      base: pr.base.ref,
      head: pr.head.ref,
      state: pr.state,
      updated_at: pr.updated_at,
      created_at: pr.created_at
    }));

    console.log(`✅ Successfully fetched ${processedPRs.length} PRs from last 24h (out of ${prs.length} total)`);
    return processedPRs;
  } catch (error) {
    console.log(`::warning::Failed to fetch PRs: ${error.message}`);
    return [];
  }
}

// Main function
async function main() {
  console.log(`📊 Generating daily report for ${REPO}`);
  console.log(`📅 Date: ${TODAY}`);
  console.log(`🌿 Branch: ${BRANCH}`);
  console.log(`⏰ Time range: Last 24 hours (since ${twentyFourHoursAgo.toISOString()})`);

  // Fetch data in parallel
  const [commits, prs] = await Promise.all([
    fetchCommits(),
    fetchPRs()
  ]);

  // Generate payload
  const payload = {
    date: TODAY,
    repository: REPO,
    branch: BRANCH,
    time_range: '24h',
    range_start: twentyFourHoursAgo.toISOString(),
    range_end: new Date().toISOString(),
    commits,
    prs
  };

  // Write to file
  const outputPath = process.env.OUTPUT_PATH || 'payload.json';
  fs.writeFileSync(outputPath, JSON.stringify(payload, null, 2));
  
  console.log(`\n📊 Summary:`);
  console.log(`   Commits (last 24h): ${commits.length}`);
  console.log(`   PRs (last 24h): ${prs.length}`);
  console.log(`\n✅ Payload written to ${outputPath}`);
  
  // Optionally print payload
  if (process.env.SHOW_PAYLOAD === 'true') {
    console.log('\n📄 Payload:');
    console.log(JSON.stringify(payload, null, 2));
  }
}

// Run
main().catch(error => {
  console.error(`::error::${error.message}`);
  process.exit(1);
});
