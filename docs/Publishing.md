# Publishing LofiBeats

This document outlines the steps to publish LofiBeats as a .NET global tool.

## Prerequisites

- .NET 9.0 SDK or later
- NuGet account (for publishing to nuget.org)
- NuGet API key (for publishing to nuget.org)

## Local Publishing

1. **Build the Solution**
   ```bash
   dotnet build -c Release
   ```

2. **Pack the CLI Tool**
   ```bash
   dotnet pack src/LofiBeats.Cli/LofiBeats.Cli.csproj -c Release
   ```
   This will create a NuGet package in `src/LofiBeats.Cli/nupkg/LofiBeats.Cli.1.0.0.nupkg`

3. **Test Locally**
   ```bash
   # Uninstall any existing version
   dotnet tool uninstall --global LofiBeats.Cli
   
   # Install from local nupkg
   dotnet tool install --global --add-source src/LofiBeats.Cli/nupkg LofiBeats.Cli
   
   # Test the installation
   lofi version
   ```

## Publishing to NuGet.org

1. **Update Version**
   - Open `src/LofiBeats.Cli/LofiBeats.Cli.csproj`
   - Update the `<Version>` tag with the new version number
   - Follow [Semantic Versioning](https://semver.org/)

2. **Build and Pack**
   ```bash
   dotnet build -c Release
   dotnet pack src/LofiBeats.Cli/LofiBeats.Cli.csproj -c Release
   ```

3. **Publish to NuGet**
   ```bash
   dotnet nuget push src/LofiBeats.Cli/nupkg/LofiBeats.Cli.{version}.nupkg --api-key {your-api-key} --source https://api.nuget.org/v3/index.json
   ```
   Replace `{version}` with your package version and `{your-api-key}` with your NuGet API key.

4. **Verify Publication**
   - Visit [nuget.org](https://www.nuget.org) and search for "LofiBeats.Cli"
   - Wait a few minutes for the package to be indexed
   - Try installing from NuGet:
     ```bash
     dotnet tool install --global LofiBeats.Cli
     ```

## GitHub Release

1. **Create a Tag**
   ```bash
   git tag -a v{version} -m "Release version {version}"
   git push origin v{version}
   ```

2. **Create GitHub Release**
   - Go to GitHub repository
   - Click "Releases" > "Create a new release"
   - Choose the tag you just created
   - Add release notes detailing changes
   - Attach the `.nupkg` file
   - Publish release

## Release Checklist

Before publishing:

- [ ] Update version number in `.csproj`
- [ ] Update CHANGELOG.md
- [ ] Run all tests
- [ ] Test package locally
- [ ] Create git tag
- [ ] Create GitHub release
- [ ] Publish to NuGet
- [ ] Verify installation from NuGet

## Troubleshooting

### Common Issues

1. **Package Not Found**
   - Ensure the package was successfully uploaded to NuGet
   - Check if the package is indexed (can take a few minutes)
   - Verify the package ID and version

2. **Installation Fails**
   - Check .NET SDK version compatibility
   - Verify all dependencies are correctly specified
   - Check for any missing files in the package

3. **Version Conflict**
   - Ensure you're not trying to publish a version that already exists
   - Use `dotnet tool update` instead of `install` if updating

### Getting Help

If you encounter issues:

1. Check the [GitHub Issues](https://github.com/willibrandon/LofiBeats/issues)
2. Review the NuGet package details on nuget.org
3. Contact the maintainers through GitHub 