# JSONSchemas & Generator

Public repository of Resonite JSON schemas that are **automatically generated** from Resonite's Source

## What are JSON Schemas?

JSON Schemas are files which describe the layout and schema of a JSON file. You can use them to Generate Code, Validate JSON etc.

## Why are these here?

When creating headless configuration files, or Resonite Client configuration files you might be unsure what you can do. These files are designed to fix this :)

## How can I use these?

We recommend using VS Code to edit your JSON files, it will then be automatic! See [VSCode's guide](https://code.visualstudio.com/Docs/languages/json#_json-schemas-and-settings) for more information. There are other tools you can use but we've had mixed success with these. The JSON Schema website has a [list of tooling](https://json-schema.org/implementations.html).

## What schemas do you have?

- [HeadlessConfig.schema.json](schemas/HeadlessConfig.schema.json) - For [headless configuration](https://wiki.resonite.com/Headless_Client/Configuration_File)
- [Config.schema.json](schemas/AppConfig.schema.json) - For [client configuration files](https://wiki.resonite.com/Startup_Config_File)

## Where can I learn more?

Head to the official [JSON Schema website](https://json-schema.org/)!

## Contributing
We accept PRs to the Readme, Generator and issues describing problems with the schemas.

We do not accept PRs to the Schemas themselves, as these schemas are automatically generated from Resonite Source. If you notice an error in one, open an issue here and we'll endeavor to fix the problem at the source.
