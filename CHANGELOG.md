# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog][Keep a Changelog] and this project adheres to [Semantic Versioning][Semantic Versioning].

## [Unreleased]

### Changed
- Split up `PhilomenaImageSearchQuery` to decouple logic
    - Add new extension methods and fluent builders to simplify use of the new classes
- Move image related classes to `Images` namespace
- Download images to temp files before moving to target files

## [1.0.0-alpha3] - 2021-02-16

### Added
- SvgMode option to image downloads
    - Allows downloading only SVG images, only rasters, or both

### Changed
- Use `null` instead of `0` when `BytesTotal` in a download is unknown
- Return null when `IPhilomenaImage` properties are missing instead of throwing an exception
- Prevent direct access to api model from IPhilomenaImage interface
    - Properties are exposed that abstract the model

### Fixed
- Download progress for files not reporting until the entire file has downloaded

## [1.0.0-alpha2] - 2021-01-25

### Added
- Generic stream download methods for image searches
- Progress reporting for image and search downloads

### Changed
- Use string paths instead of FileInfo
- `FilterImagesDelegate` to `ShouldDownloadImageDelegate`

### Fixed
- One metadata download after limit reached if limit is a multiple of page size

## [1.0.0-alpha1] - 2020-12-29

### Added
- Philomena Client with support for
    - Building image search queries
    - Processing image search results
    - Downloading images in a query
    - Parallel image downloads
    - Custom image download filters
    - Getting a tag by ID
- Examples of downloading images and tags
- CI build and deploy

<!-- Links -->
[Keep a Changelog]: https://keepachangelog.com/
[Semantic Versioning]: https://semver.org/

<!-- Versions -->
[Unreleased]: https://github.com/Sibusten/philomena-client/compare/v1.0.0-alpha3...HEAD
[1.0.0-alpha3]: https://github.com/Sibusten/philomena-client/compare/v1.0.0-alpha2..v1.0.0-alpha3
[1.0.0-alpha2]: https://github.com/Sibusten/philomena-client/compare/v1.0.0-alpha1..v1.0.0-alpha2
[1.0.0-alpha1]: https://github.com/Sibusten/philomena-client/releases/v1.0.0-alpha1
