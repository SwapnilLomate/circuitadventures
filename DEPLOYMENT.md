# GitHub Pages Deployment Guide

This guide explains how to deploy Circuit Adventures to GitHub Pages.

## Prerequisites

- A GitHub account
- Your repository pushed to GitHub
- GitHub Pages enabled in repository settings

## Automatic Deployment Setup

The project includes a GitHub Actions workflow that automatically deploys to GitHub Pages on every push to the `main` branch.

### Step 1: Enable GitHub Pages

1. Go to your GitHub repository
2. Click on **Settings** → **Pages**
3. Under **Source**, select **GitHub Actions**
4. Save the settings

### Step 2: Configure Repository Permissions

The workflow needs proper permissions to deploy:

1. Go to **Settings** → **Actions** → **General**
2. Scroll to **Workflow permissions**
3. Select **Read and write permissions**
4. Check **Allow GitHub Actions to create and approve pull requests**
5. Click **Save**

### Step 3: Push to Main Branch

Once you push to the `main` branch, the GitHub Action will automatically:

1. Build the Blazor WebAssembly application
2. Configure the base path for GitHub Pages
3. Add required files (`.nojekyll`, `404.html`)
4. Deploy to GitHub Pages

```bash
git add .
git commit -m "Setup GitHub Pages deployment"
git push origin main
```

### Step 4: Monitor Deployment

1. Go to **Actions** tab in your repository
2. Watch the "Deploy to GitHub Pages" workflow run
3. Once complete, your site will be live at:
   ```
   https://<username>.github.io/<repository-name>/
   ```

## Manual Deployment (Alternative)

If you prefer to deploy manually:

```bash
# Build and publish the project
dotnet publish CircuitAdventures.Client/CircuitAdventures.Client.csproj -c Release -o release

# Update base path in index.html
# Replace <repository-name> with your actual repo name
sed -i 's/<base href="\/" \/>/<base href="\/<repository-name>\/" \/>/g' release/wwwroot/index.html

# Add .nojekyll file
touch release/wwwroot/.nojekyll

# Copy index.html as 404.html for SPA routing
cp release/wwwroot/index.html release/wwwroot/404.html

# Deploy the wwwroot folder to GitHub Pages
# (Use your preferred method: gh-pages branch, GitHub CLI, etc.)
```

## Configuration Details

### Base Path

The GitHub Action automatically adjusts the base path in `index.html` to match your repository name:

```html
<!-- Before -->
<base href="/" />

<!-- After (for repo named 'circuitadventures') -->
<base href="/circuitadventures/" />
```

### .nojekyll File

This file tells GitHub Pages not to process the site with Jekyll, which is necessary for Blazor apps to work correctly.

### 404.html

For proper SPA routing, the `404.html` is a copy of `index.html`. This ensures that direct navigation to routes works correctly.

## Workflow Configuration

The workflow file is located at `.github/workflows/deploy-to-github-pages.yml`

**Trigger Events:**
- Push to `main` branch
- Manual workflow dispatch (via Actions tab)

**Permissions Required:**
- `contents: read` - Read repository contents
- `pages: write` - Write to GitHub Pages
- `id-token: write` - Authenticate with GitHub Pages

## Troubleshooting

### Site Not Loading

1. **Check GitHub Actions logs** for build errors
2. **Verify base path** in deployed `index.html` matches your repo name
3. **Confirm .nojekyll** file exists in deployed files

### Routing Issues

1. Ensure `404.html` exists and is a copy of `index.html`
2. Check that base path is correctly configured

### Permission Errors

1. Verify workflow permissions in repository settings
2. Check that GitHub Pages source is set to "GitHub Actions"

### Asset Loading Failures

1. Ensure all paths in your code use relative paths
2. Verify that the base href is correctly set
3. Check browser console for 404 errors

## Custom Domain (Optional)

To use a custom domain:

1. Add a `CNAME` file to `CircuitAdventures.Client/wwwroot/` with your domain
2. Configure DNS settings with your domain provider
3. Update base href in `index.html` to `<base href="/" />`
4. Modify the workflow to skip the base-tag change step

## Local Testing

To test the production build locally:

```bash
# Build and publish
dotnet publish CircuitAdventures.Client -c Release -o ./publish

# Serve the wwwroot folder
cd publish/wwwroot
python -m http.server 8080

# Open http://localhost:8080 in your browser
```

## Maintenance

The GitHub Action runs automatically on every push to `main`. To manually trigger:

1. Go to **Actions** tab
2. Select "Deploy to GitHub Pages" workflow
3. Click **Run workflow**
4. Choose the branch and click **Run workflow**

## Performance Considerations

- First load may take a few seconds (Blazor WebAssembly initialization)
- Static assets are cached by GitHub Pages CDN
- Consider enabling compression for faster loads
- Service Worker can be added for offline support

## Security Notes

- All client-side code is visible (Blazor WebAssembly runs in browser)
- Don't include sensitive data, API keys, or secrets in the client code
- Use backend APIs for sensitive operations
- GitHub Pages serves content over HTTPS automatically

---

**Need Help?** Check the [Blazor documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly) or [GitHub Pages documentation](https://docs.github.com/en/pages).
