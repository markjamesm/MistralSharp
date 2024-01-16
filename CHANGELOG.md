# Changelog

## 1.0.3 - 2024-01-15

- Renamed parameter `SafeMode` as `SafePrompt` - [Mistral REST API was throwing a 422 since it has been made stricter](https://discord.com/channels/1144547040454508606/1184444810279522374/1195108690353717369)
- Replaced instances of List<> with IEnumerable<> for improved performance.

## 1.0.2 - 2023-12-20

- Added more descriptive error messages for HttpClient errors.

## 1.0.1 - 2023-12-16

- Refactored MistralClient class.

## 1.0.0 - 2023-12-16

- All Mistral REST API endpoints are now supported.
- Added support for .NET Standard 2.0.

## 0.1.1 - 2023-12-11

- Minor code cleanup.

## 0.1.0 - 2023-12-11

_First Release._

- Support for ListModels and Chat endpoints.