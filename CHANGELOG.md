# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.0] - 2022-06-15

### Added

- [@JShull](https://github.com/jshull).
  - Moved all test files to a Unity Package Distribution
  - Setup the ChangeLog.md
  - Setup the Package Layout according to [Unity cus-Layout](https://docs.unity3d.com/Manual/cus-layout.html)
  - Humble Design has been started but isn't fully implemented
    - SequenceItem.cs currently contains SequenceLogic as a Humble Pattern Data/Logics Class
      - Still not sure on how this is going to flush out as I have a lot of custom editor work going on
  - Editor contains a EditorUtil.cs which has a lot of helpful and useful methods for helping bring in added visual information to the editor scripts associated with the SequenceItem.cs
  - Added LICENSE.MD under a dual license for education and for business use cases

### Changed

- Script Naming Conventions to match Unity Packages
- Using Unity 2021.2.19f1 to build this package, should work in lower levels of Unity but I haven't tested it.

### Fixed

- Setup the contents to align with Unity naming conventions
- Had to modify existing Assembly Definition files associated with the Tests
- Hot Fix removed unneeded *.asmdef reference in the runtime folder, no version change

### Removed
