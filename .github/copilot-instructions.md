# CircuitAdventures - AI Coding Agent Instructions

## Project Overview
CircuitAdventures is a Blazor WebAssembly educational app teaching electronics to children through 100 progressive hands-on projects. Built with .NET 9, using client-side only architecture with localStorage persistence.

## Key Architecture Patterns

### Service Layer Design
- **Singleton services**: All application services (`UserService`, `LevelService`, `ProgressService`, `CertificateService`, `LocalStorageService`) are registered as singletons in `Program.cs` for data persistence across component lifecycles
- **Service composition**: `ProgressService` orchestrates between `UserService` and `CertificateService` for level completion workflows
- **Event-driven updates**: `UserService.OnUserDataChanged` event notifies components of data changes for reactive UI updates

### Data Persistence Strategy
- **Browser localStorage only**: No backend - all user data persists via `Blazored.LocalStorage`
- **Dual storage**: Main data in `circuitadventures-user` key, auto-backup in `circuitadventures-backup` key
- **Immediate saves**: All user actions (level completion, quiz answers, progress) trigger immediate `SaveUserDataAsync()` calls
- **Error handling**: All localStorage operations wrapped in try-catch with console logging

### Component Organization
```
Components/
├── Onboarding/          # User registration flow (empty - needs implementation)
├── [Shared components]  # Reusable UI components (planned)
Pages/
├── Home.razor          # Main dashboard
├── Counter.razor       # Demo page (to be replaced)
├── Weather.razor       # Demo page (to be replaced)
Models/
├── UserData.cs         # Core user state model
├── Level.cs           # Project definition with components, instructions, quiz
├── Certificate.cs     # Milestone achievements
Data/
├── LevelsData.cs      # Static method GetLevels() - currently has 2 sample levels
```

### Level System Architecture
- **Sequential progression**: Users can only access current level + completed levels (no skipping ahead)
- **Star rating**: 1-3 stars based on completion (1=completed, 2=+quiz correct, 3=+challenge mode)
- **Certificate milestones**: Auto-awarded every 10 levels (10, 20, 30... 100)
- **Level structure**: Each level includes components list, step-by-step instructions, safety notes, quiz question, optional challenge mode

## Development Conventions

### Data Flow Patterns
1. **Initialization**: Components call `UserService.InitializeAsync()` to load localStorage data
2. **State checks**: Use `UserService.IsUserOnboardedAsync()` to determine if user needs onboarding
3. **Progress updates**: Call `ProgressService.CompleteLevelAsync(levelId, stars)` which handles level advancement, certificate awards, and streak updates
4. **UI reactivity**: Subscribe to `UserService.OnUserDataChanged` for component re-rendering

### Critical Implementation Details
- **Service lifetimes**: Services must be Singleton to maintain state across navigation
- **Async patterns**: All localStorage operations are async - always use `await` for data operations
- **Error resilience**: LocalStorageService returns null on errors; components should handle null UserData gracefully
- **Level progression**: `UserData.CurrentLevel` determines next available level; `CompletedLevels` list tracks finished projects

### Technology Stack Specifics
- **UI Framework**: MudBlazor 8.13.0 + Bootstrap 5 for responsive design
- **State Management**: Service injection + event notifications (no external state library)
- **Asset organization**: SVG diagrams in `/wwwroot/images/levels/level-XXX/` structure
- **Component styling**: Scoped CSS files (`.razor.css`) for component-specific styles

## Essential Workflows

### User Onboarding Flow (Not Implemented)
1. Check `UserService.IsUserOnboardedAsync()` on app load
2. If false, show onboarding components: `WelcomeScreen` → `NameEntry` → optional `LevelSelector`
3. Call `UserService.CreateUserAsync(name, avatar, startingLevel)` to persist user

### Level Completion Flow
1. User builds project following `Level.Instructions` steps
2. Click "I've Built This!" triggers quiz modal with `Level.Quiz`
3. Award stars based on performance, call `ProgressService.CompleteLevelAsync()`
4. If milestone (level % 10 == 0), certificate modal displays new achievement
5. Show prominent "Go to Next Level" button for continued engagement

### Certificate System
- Certificates auto-generated with user name and earned date at levels 10, 20, 30... 100
- Each has unique name: "Spark Starter Certificate" (10), "Circuit Champion Certificate" (90), etc.
- Stored in `UserData.Certificates` list, viewable/downloadable from progress page

## Code Quality Expectations
- **XML documentation**: All public methods require `<summary>` comments
- **Null safety**: Enable nullable reference types, handle null UserData scenarios
- **Accessibility**: Semantic HTML, ARIA labels for interactive elements
- **Child-friendly**: Large touch targets (44px min), high contrast colors, simple language
- **Performance**: Lazy load images, optimize component images, fast initial load

## Current Implementation Status
- ✅ Service architecture with DI setup
- ✅ Data models and localStorage persistence
- ✅ Basic routing and layout structure
- ✅ 2 sample levels with full data structure
- ❌ Onboarding components (empty folder)
- ❌ Level detail pages and quiz modals
- ❌ Certificate generation and viewing
- ❌ Progress tracking UI
- ❌ 98 remaining levels with content

Focus development on completing the user onboarding flow, then level detail pages with quiz functionality, followed by certificate system implementation.