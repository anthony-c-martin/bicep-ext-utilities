# Bicep Utilities Extension

## Usage

1. Download the [Samples folder](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2Fanthony-c-martin%2Fbicep-ext-utilities%2Ftree%2Fmain%2Fsamples), and unzip it.
1. Open the unzipped Samples folder in VSCode, and select one of the `.bicepparam` files you wish to deploy.
1. Launch the [Deploy Pane](https://github.com/Azure/bicep/blob/main/docs/experimental/deploy-ui.md) to run the deployment.

> [!NOTE]
> Extension binary packages are not signed on a Mac. If you see the following error, you will need to manually sign the extension package:
> 
> `Failed to launch provider: Failed to connect to provider .../extension.bin`
> 
> To work around it, run the following in a terminal window, using the path from the error message:
> 
> `codesign -s - '<path from the error message>'`

## Build + Test Locally

### Rebuild the extension
These commands publish the extension to the local file system, and updates the sample bicepconfig to point to the local extension.
```sh
./scripts/publish.sh ./bin/bicep-ext-utilities
jq '.extensions.utilities="../bin/bicep-ext-utilities"' ./samples/bicepconfig.json > ./samples/bicepconfig.new.json
mv ./samples/bicepconfig.new.json ./samples/bicepconfig.json
```

### Test the extension
Run the deployment.
```sh
~/.azure/bin/bicep local-deploy ./samples/basic/main.bicepparam
```

To enable verbose tracing, run the following beforehand.

```sh
export BICEP_TRACING_ENABLED=true
```

When using PowerShell, use the snippet below to enable verbose tracing:
```pwsh
$env:BICEP_TRACING_ENABLED = "true"
```

## Releasing

Releases are cut manually so that versioning stays under explicit control. Run the **Release** workflow from the Actions tab (or with `gh workflow run release.yml -f version=0.2.0`) and supply the exact version to publish.

The workflow validates the version, builds and tests, publishes `br:ghcr.io/anthony-c-martin/bicep-ext-utilities:<version>`, and then creates a `v`-prefixed git tag and GitHub Release. The workflow refuses to replace an existing release and only runs from `main`.

The version supplied to the workflow is stamped into the binary via `-p:Version=`. Local builds use the placeholder `0.0.1-dev` version from [src/Bicep.Extension.Utilities.csproj](./src/Bicep.Extension.Utilities/Bicep.Extension.Utilities.csproj).

To configure this repository's GitHub branch protection and collaborators, log in with the `gh` CLI and run:

```powershell
./scripts/setup.ps1
```

## Building other extensions

This repo is also intended to demonstrate how to build + publish an end-to-end Bicep extension in C#. Feel free to copy, rename and modify it to prototype building an extension to extend other services.
