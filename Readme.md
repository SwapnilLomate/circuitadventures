# CircuitAdventures - Project Specification

## Project Overview
An educational web application for an 8-year-old (Ayansh) and parent to learn electronics through 100 progressive hands-on projects using DIY electronic components.

**Project Name**: CircuitAdventures
**Website/App Name**: CircuitAdventures

## Target Users
- Primary: 8-year-old child learning electronics
- Secondary: Parent guiding the learning process
- Age-appropriate UI/UX with colorful, engaging design

## User Onboarding & Data Management

### First Visit Experience
1. **Welcome Screen**
   - Friendly mascot/robot character
   - "Welcome to CircuitAdventures!" greeting
   - Brief explanation of what they'll learn

2. **Name Entry**
   - Simple form: "What's your name, young inventor?"
   - Large, kid-friendly input field
   - Optional: Choose an avatar/icon
   - Validation: Name required (2-20 characters)

3. **Returning User Option**
   - After entering name, show: "Is this your first time?"
   - If No: "Which level would you like to start from?"
   - Dropdown/slider to select level (1-100)
   - This allows users to recover progress if localStorage was cleared

4. **Data Storage Notice**
   - Simple message for parents: "Your progress is saved on this device"
   - Explanation that clearing browser data will reset progress
   - Recommendation to always use the same device

### Local Storage Architecture
All user data stored in browser localStorage via Blazored.LocalStorage with the following structure:

```csharp
public class UserData
{
    public string Name { get; set; }
    public string Avatar { get; set; } // optional avatar choice
    public DateTime FirstVisit { get; set; }
    public DateTime LastVisit { get; set; }
    public int CurrentLevel { get; set; } = 1; // 1-100
    public List<int> CompletedLevels { get; set; } = new();
    public Dictionary<int, int> StarsEarned { get; set; } = new(); // level ID -> stars (1-3)
    public List<Certificate> Certificates { get; set; } = new();
    public int TotalTimeSpent { get; set; } // minutes
    public StreakData Streak { get; set; } = new();
    public List<string> Badges { get; set; } = new();
    public Dictionary<int, string> Notes { get; set; } = new();
    public UserSettings Settings { get; set; } = new();
}

public class Certificate
{
    public string Id { get; set; }
    public string Name { get; set; } // certificate title
    public int Level { get; set; } // milestone level (10, 20, 30, etc.)
    public DateTime EarnedDate { get; set; }
    public string UserName { get; set; }
}

public class StreakData
{
    public int Current { get; set; }
    public int Longest { get; set; }
    public DateTime LastActiveDate { get; set; }
}

public class UserSettings
{
    public bool SoundEnabled { get; set; } = true;
    public string TextSize { get; set; } = "medium"; // small, medium, large
    public bool DarkMode { get; set; } = false;
}
```

### LocalStorage Keys
- `circuitadventures-user`: Main user data
- `circuitadventures-backup`: Backup of last session (auto-created)

## Available Components
- Motors (various types)
- Batteries
- Solar panels
- Fans
- Dynamos
- Solar motors
- LEDs
- Jumper wires
- Circuit breadboard
- Capacitors
- Resistors
- Switches
- Small bulbs
- Additional craft materials: cardboard, paper, tape, glue

## Visual Content Strategy
**Diagram-Focused Approach**: The app will use high-quality, clear circuit diagrams and illustrations instead of photographs. This approach provides:
- Clearer component identification
- Better visibility of connections
- Less visual clutter
- Easier to understand wire routing
- Consistent visual style
- Fully generatable by AI

All diagrams will be created as SVG files with:
- Color-coded wires (red for positive, black for negative, other colors for signal)
- Clearly labeled components
- Step-by-step highlighting capability
- Multiple view angles where needed
- Zoom and pan functionality
- Print-friendly versions

## Technology Stack

### Framework
- **Blazor WebAssembly** (Client-side Blazor)
- **.NET 8.0** or latest LTS version
- **C#** for all application logic

### Styling
- **Bootstrap 5** for responsive design and UI components
- **Custom CSS** for kid-friendly, colorful design
- Responsive design (mobile-first approach)

### State Management
- Blazor's built-in state management
- Service-based architecture for data management
- Browser localStorage via JSInterop for persistence

### Key Libraries & NuGet Packages
- **Blazored.LocalStorage** - For browser localStorage access
- **MudBlazor** (optional) - Material Design component library for rich UI
- **Blazor.Extensions.Canvas** - For interactive diagram rendering
- **SVG rendering** - Built-in Blazor SVG support

### JavaScript Interop
- Minimal JS for:
  - localStorage operations (via Blazored.LocalStorage)
  - Certificate download (html2canvas equivalent)
  - Diagram zoom/pan interactions
  - Print functionality

## Application Structure

```
CircuitAdventures/
├── CircuitAdventures.Client/          # Blazor WebAssembly project
│   ├── wwwroot/
│   │   ├── css/
│   │   │   ├── app.css
│   │   │   └── bootstrap/
│   │   ├── images/
│   │   │   ├── levels/
│   │   │   │   ├── level-001/
│   │   │   │   │   ├── main-diagram.svg
│   │   │   │   │   ├── step-1.svg
│   │   │   │   │   ├── step-2.svg
│   │   │   │   │   └── final-view.svg
│   │   │   │   └── ... (up to level-100)
│   │   │   ├── components/
│   │   │   │   ├── led.svg
│   │   │   │   ├── motor.svg
│   │   │   │   ├── battery.svg
│   │   │   │   └── ... (all component illustrations)
│   │   │   └── badges/
│   │   │       ├── beginner.svg
│   │   │       └── ...
│   │   ├── js/
│   │   │   ├── diagram-viewer.js
│   │   │   └── certificate-download.js
│   │   └── index.html
│   ├── Pages/
│   │   ├── Index.razor                 # Home page
│   │   ├── LevelMap.razor              # All levels overview
│   │   ├── LevelDetail.razor           # Individual level page
│   │   ├── Progress.razor              # Progress tracking
│   │   ├── Certificates.razor          # Certificate gallery
│   │   ├── Components.razor            # Component library
│   │   └── About.razor
│   ├── Shared/
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor
│   │   ├── Header.razor
│   │   └── Footer.razor
│   ├── Components/
│   │   ├── Onboarding/
│   │   │   ├── WelcomeScreen.razor
│   │   │   ├── NameEntry.razor
│   │   │   └── LevelSelector.razor
│   │   ├── LevelCard.razor
│   │   ├── CompletionModal.razor
│   │   ├── CertificateModal.razor
│   │   ├── CertificateCard.razor
│   │   ├── CertificateViewer.razor
│   │   ├── ProgressBar.razor
│   │   ├── BadgeDisplay.razor
│   │   ├── DiagramViewer.razor
│   │   ├── SafetyTips.razor
│   │   ├── QuizQuestion.razor
│   │   └── GoToNextButton.razor
│   ├── Services/
│   │   ├── UserService.cs              # Manages user data
│   │   ├── ProgressService.cs          # Tracks progress
│   │   ├── LevelService.cs             # Level data management
│   │   ├── CertificateService.cs       # Certificate generation
│   │   ├── LocalStorageService.cs      # LocalStorage wrapper
│   │   └── NavigationService.cs        # Navigation helper
│   ├── Models/
│   │   ├── UserData.cs
│   │   ├── Level.cs
│   │   ├── Certificate.cs
│   │   ├── Component.cs
│   │   ├── Badge.cs
│   │   └── InstructionStep.cs
│   ├── Data/
│   │   ├── LevelsData.cs               # All 100 levels
│   │   ├── ComponentsData.cs           # Component definitions
│   │   ├── BadgesData.cs               # Badge definitions
│   │   └── CertificatesData.cs         # Certificate templates
│   ├── Helpers/
│   │   └── Utilities.cs
│   ├── _Imports.razor
│   ├── App.razor
│   ├── Program.cs
│   └── CircuitAdventures.Client.csproj
└── CircuitAdventures.sln
```

## Core Features

### 1. Level System (100 Levels)

#### Level Categories
- **Beginner Zone (1-20)**: Basic circuits, LEDs, switches
- **Energy Explorer (21-35)**: Solar panels, dynamos, capacitors
- **Motion Maker (36-50)**: Motors, movement, simple machines
- **Light Master (51-65)**: Advanced LED circuits, patterns
- **Circuit Wizard (66-80)**: Complex multi-component circuits
- **Innovation Lab (81-100)**: Creative challenges, final projects

#### Level Data Structure
```csharp
public class Level
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public int Difficulty { get; set; } // 1-5
    public int EstimatedTime { get; set; } // minutes
    public List<ComponentItem> Components { get; set; }
    public List<string> AdditionalMaterials { get; set; }
    public List<string> LearningObjectives { get; set; }
    public List<string> SafetyNotes { get; set; }
    public List<InstructionStep> Instructions { get; set; }
    public List<string> Diagrams { get; set; }
    public string FunFact { get; set; }
    public Challenge ChallengeMode { get; set; }
    public int? UnlockRequirement { get; set; } // previous level ID
}

public class ComponentItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
}

public class InstructionStep
{
    public int StepNumber { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string DiagramUrl { get; set; } // SVG diagram for this step
    public string Tip { get; set; }
    public string Warning { get; set; }
}

public class Challenge
{
    public string Description { get; set; }
    public string SuccessCriteria { get; set; }
}
```

### 2. Progress Tracking

```csharp
public class UserProgress
{
    public List<int> CompletedLevels { get; set; } = new();
    public int CurrentLevel { get; set; } = 1;
    public Dictionary<int, int> StarsEarned { get; set; } = new(); // level ID -> stars (1-3)
    public List<string> Badges { get; set; } = new();
    public int TotalTimeSpent { get; set; } // minutes
    public List<int> FavoriteLevels { get; set; } = new();
    public Dictionary<int, string> Notes { get; set; } = new(); // level ID -> note
}
```

#### Star Rating System
- ⭐ 1 Star: Completed the project
- ⭐⭐ 2 Stars: Completed + answered quiz question correctly
- ⭐⭐⭐ 3 Stars: Completed + quiz + completed challenge mode

#### Progress Flow
1. User completes level instructions
2. Clicks "Mark as Complete" button
3. Mini quiz appears (1 question)
4. Stars awarded based on performance
5. **"Go to Next Level" button appears**
6. Progress automatically saved to localStorage
7. If milestone reached (10, 20, 30, etc.), certificate modal appears
8. Next level unlocked automatically

### 3. Main Pages

#### Home Page
- Welcome message with user's name: "Welcome back, [Name]!"
- Fun mascot/character
- **"Continue Learning" button** (goes to current level)
- Quick stats: levels completed, current streak
- Featured achievement/badge
- Preview of next certificate milestone
- Safety reminder section

#### Level Map Page
- Visual map showing all 100 levels
- Color-coded by category
- Locked/unlocked states (only current level + completed levels accessible)
- Progress indicators (stars)
- Current level highlighted
- Filter by category
- Search functionality
- Jump to level option (in settings, for recovery purposes)

#### Level Detail Page
- Level title and category badge
- Difficulty indicator
- Estimated time
- Components needed (with component illustrations)
- Additional materials list
- Safety notes (prominently displayed)
- Tabbed sections:
  - **Instructions**: Step-by-step guide with diagrams
  - **Circuit Diagram**: Full circuit view with zoom/pan
  - **3D View**: Isometric/perspective view of assembly (optional)
  - **Learn More**: Educational content about concepts
- **Completion Flow**:
  1. "I've Built This!" button at bottom
  2. Quiz question modal appears
  3. Stars awarded animation
  4. **"Go to Next Level" button** (large, prominent)
  5. If milestone (10,20,30, etc.), certificate earned modal
- Challenge mode toggle
- Notes section (for kids to write what they learned)
- Print button (for offline building)
- Navigation: Previous Level | Level Map | Next Level (if unlocked)

#### Progress Page
- Overall completion percentage
- Levels completed by category (pie chart)
- **Certificates Section**:
  - Grid display of all earned certificates
  - Locked certificates (grayed out, showing requirements)
  - Click certificate to view full size
  - Download button for each earned certificate (as PNG or PDF)
- Badges earned (visual display)
- Learning streak calendar
- Time spent on projects
- Favorite projects list
- Data management: "Reset Progress" button (with confirmation)

#### Certificates Gallery Page
- Dedicated page for viewing all certificates
- Large, beautiful display of earned certificates
- Download individual or download all option
- Share certificate feature (copy as image)
- Print certificate option
- Progress tracker showing next certificate milestone

#### Components Library Page
- Grid of all available components
- Each component card shows:
  - Image
  - Name
  - What it does (simple explanation)
  - Safety information
  - Which levels use it
- Search and filter functionality

### 4. Key UI Elements

#### Navigation
- Colorful, kid-friendly header
- User name displayed with greeting
- Large, easy-to-click buttons
- Progress indicator in header (X/100 levels)
- Quick access to current level
- Certificate count indicator

#### Level Cards
- Large, colorful cards
- Clear difficulty indicators
- Component icons preview
- **Lock icon for unavailable levels** (can't skip ahead)
- Star rating display (earned stars vs possible)
- "Current Level" badge on active level
- Hover effects for interactivity
- Completion checkmark for finished levels

#### Completion Modal
**After clicking "I've Built This!":**
1. Celebration animation (confetti, sparkles)
2. Quiz question appears
3. Answer feedback with explanation
4. Stars awarded animation (1-3 stars)
5. **"Go to Next Level" button** (primary CTA)
6. "Return to Level Map" button (secondary)
7. If milestone reached: "🎉 You earned a certificate!" banner

#### Certificate Modal
**When milestone reached (10, 20, 30, 40, 50, 60, 70, 80, 90, 100):**
- Full-screen celebration
- Certificate slides in with animation
- Certificate displays:
  - **Certificate Name** (see certificate names below)
  - "Presented to [User Name]"
  - "For completing [X] levels in Circuit Adventures"
  - Date of achievement
  - Decorative border and designs
  - Circuit Adventures logo
  - Level milestone number
- Buttons:
  - "Download Certificate" (PNG/PDF)
  - "Continue Learning"
  - "View All Certificates"

#### Diagram Viewer
- Zoom in/out functionality (pinch and buttons)
- Pan and navigate
- Interactive labels for components (hover/tap to see info)
- Step-by-step highlighting option
- Toggle wire colors for clarity
- Show/hide component labels
- Rotation controls for 3D views
- Fullscreen mode
- Printable version (optimized for paper)
- Export as image option

#### Safety Banner
- Always visible during instructions
- Age-appropriate safety reminders
- "Ask an adult" prompts when needed
- Emergency stop instructions

### 5. Gamification Features

#### Certificate System
Certificates awarded at milestones: 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 levels

**Certificate Names (Issued by CircuitAdventures):**
- **10 Levels**: "Spark Starter Certificate" - For igniting your journey into electronics
- **20 Levels**: "Current Explorer Certificate" - For exploring the flow of electricity
- **30 Levels**: "Circuit Apprentice Certificate" - For mastering basic circuits
- **40 Levels**: "Voltage Voyager Certificate" - For voyaging through power and energy
- **50 Levels**: "Motion Magician Certificate" - For bringing circuits to life with movement
- **60 Levels**: "Light Illuminator Certificate" - For brilliantly mastering LED circuits
- **70 Levels**: "Power Pioneer Certificate" - For pioneering advanced electronics
- **80 Levels**: "Innovation Inventor Certificate" - For inventing creative solutions
- **90 Levels**: "Circuit Champion Certificate" - For championing complex circuits
- **100 Levels**: "Master Electrician Certificate" - For mastering all of CircuitAdventures

**Certificate Design:**
```csharp
public class CertificateDesign
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public string Description { get; set; }
    public string BorderColor { get; set; } // unique color per certificate
    public string IconSymbol { get; set; } // emoji or icon representing the achievement
    public string BackgroundColor { get; set; }
    public string Template { get; set; } // different visual styles
}
```

**Certificate Visual Elements:**
- Large "CircuitAdventures" logo at top
- Certificate title (e.g., "Spark Starter Certificate")
- "This certifies that" text
- **[User Name]** in prominent decorative font
- "Has successfully completed [X] levels"
- "In CircuitAdventures Electronics Learning Program"
- Date of achievement
- Decorative circuit board pattern border
- Signature line: "The CircuitAdventures Team"
- Small graphics representing the category (spark, circuit, motor, etc.)

**Certificate Templates:**
- Professional border with circuit pattern
- User name in large, decorative font
- Date of achievement
- Level milestone number
- Signature line: "CircuitAdventures Team"
- Unique serial number (generated from user name + date)
- QR code linking back to CircuitAdventures (optional)
- Downloadable as:
  - High-resolution PNG (for digital sharing)
  - PDF (for printing)
  - Shareable social media format

#### Badges/Achievements
- **First Steps**: Complete level 1
- **Light Bringer**: Complete 5 LED projects
- **Motor Master**: Complete 10 motor projects
- **Solar Pioneer**: Complete first solar project
- **Speed Demon**: Complete level in under estimated time
- **Perfectionist**: Earn 3 stars on 10 levels
- **Experimenter**: Complete all challenge modes in a category
- **Note Taker**: Write notes on 5 levels
- **Consistent Learner**: 7-day streak
- **Dedicated Builder**: 30-day streak
- **Professor**: Complete all 100 levels
- **Category-specific badges**: One for each category completed (6 total)

#### Streak System
- Daily learning streak counter
- Encouragement to keep learning
- Streak milestones (7 days, 30 days, etc.)
- Visual calendar showing active days
- Streak freeze: Allow 1 day break without losing streak (future feature)

#### Level Progression Rules
- Levels must be completed sequentially (can't skip)
- Current level + all previous levels are accessible
- Future levels are locked (shown but grayed out)
- **Exception**: First-time users can choose starting level (for recovery)
- Once a level is completed, it can be revisited anytime
- Re-doing levels can improve star rating

### 6. Educational Features

#### Learning Objectives
- Clear goals for each level
- Age-appropriate explanations
- Real-world applications
- Science behind the circuit
- Visual explanations with animated diagrams

#### Fun Facts
- Interesting tidbits about electricity
- Historical facts about inventors
- Cool applications of the concepts
- Illustrated with simple graphics

#### Quiz Questions
- 1 simple question per level
- Multiple choice format
- Educational feedback for wrong answers
- Visual hints using diagram elements
- Reward star for correct answer

### 7. Diagram Design Standards

#### Component Illustrations
- Consistent style across all components
- Realistic but simplified shapes
- Clear labeling with pointer lines
- Color-coding for component types:
  - LEDs: Yellow/Red/Green based on color
  - Resistors: Beige with color bands
  - Capacitors: Blue cylinders
  - Batteries: Black with +/- labels
  - Motors: Gray with red/black terminals
  - Wires: Red (positive), Black (negative), other colors for signals

#### Circuit Diagrams
- Top-down view for breadboard layouts
- Schematic symbol view for understanding
- Isometric/3D view for physical assembly
- Wire routing clearly visible
- Connection points highlighted
- Numbered steps overlay

#### Diagram Features
- SVG format for infinite scaling
- Embedded text (not as paths) for accessibility
- Layer-based structure for interactivity
- Consistent dimensions (1200x800px base)
- Light background for printing
- Dark mode alternative

### 8. Accessibility Features
- Large, readable fonts
- High contrast color schemes
- Text-to-speech option for instructions
- Adjustable text size
- Keyboard navigation support
- Screen reader friendly
- Alt text for all diagrams
- Color-blind friendly wire coding (patterns + colors)

### 9. Parent Features
- Progress overview dashboard
- Time spent tracking
- Skill development tracking
- Printable level sheets
- Additional resources/links
- Safety guidelines
- **Certificate viewing and download**
- Data export option (JSON format for backup)
- Data import option (restore from backup)
- Reset progress option (with strong confirmation)

## Sample Level Specifications

### Level 1: "First Light"
- **Category**: Beginner Zone
- **Difficulty**: 1/5
- **Time**: 5 minutes
- **Components**: 1 LED, 1 battery (AA), 2 jumper wires
- **Objective**: Understand basic circuit - make an LED light up
- **Instructions**: 4 simple steps with clear diagrams showing:
  - Step 1: Identify LED legs (long = positive, short = negative)
  - Step 2: Connect wire from battery + to LED long leg
  - Step 3: Connect wire from LED short leg to battery -
  - Step 4: LED lights up!
- **Diagrams**: 
  - Component identification diagram
  - Step-by-step connection diagrams
  - Final circuit diagram
- **Fun Fact**: "LEDs use 75% less energy than regular bulbs!"
- **Safety**: Batteries can get hot if short-circuited

### Level 25: "Solar Charger"
- **Category**: Energy Explorer
- **Difficulty**: 3/5
- **Time**: 20 minutes
- **Components**: Solar panel, capacitor, LED, wires, breadboard
- **Objective**: Store solar energy and use it to power an LED
- **Instructions**: 8 steps with diagrams showing breadboard layout
- **Diagrams**:
  - Breadboard layout (top view)
  - Circuit schematic
  - 3D assembly view
  - Capacitor charging indicator
- **Challenge**: Make LED stay lit for 30 seconds after covering solar panel
- **Fun Fact**: "Space satellites use solar panels just like this!"

### Level 50: "Cardboard Racer"
- **Category**: Motion Maker
- **Difficulty**: 3/5
- **Time**: 30 minutes
- **Components**: Motor, battery, switch, wires
- **Additional**: Cardboard, glue, tape, bottle caps (wheels)
- **Objective**: Build a motorized car that can race
- **Instructions**: 10 steps with diagrams for both circuit and cardboard assembly
- **Diagrams**:
  - Circuit wiring diagram
  - Cardboard cutting template (printable)
  - Assembly sequence (exploded view)
  - Final car with circuit placement
- **Challenge**: Make car travel 3 meters in a straight line
- **Fun Fact**: "Electric cars work on the same principle!"

### Level 100: "Ultimate Innovation"
- **Category**: Innovation Lab
- **Difficulty**: 5/5
- **Time**: 90+ minutes
- **Components**: All available components (user's choice)
- **Objective**: Design and build your own invention using everything learned
- **Requirements**: 
  - Must use at least 5 different component types
  - Must have 2 functions
  - Must be creative and original
- **Instructions**: Project planning guide with examples of possible creations
- **Diagrams**: Example project diagrams to inspire (not to copy)
- **Celebration**: Special completion animation and printable certificate

## Design Guidelines

### Color Scheme
- **Primary**: Bright blue (#3B82F6) - Technology/Learning
- **Secondary**: Sunny yellow (#FBBF24) - Energy/Achievement
- **Accent**: Electric green (#10B981) - Success/Go
- **Warning**: Orange (#F59E0B) - Caution
- **Danger**: Red (#EF4444) - Stop/Safety
- **Background**: Light gray (#F3F4F6)
- **Cards**: White with subtle shadows

### Typography
- **Headings**: Poppins (fun, rounded, easy to read)
- **Body**: Inter (clean, modern, readable)
- **Code/Technical**: Roboto Mono

### Icon Style
- Rounded, colorful icons
- Large touch targets (44x44px minimum)
- Consistent style throughout

### Animations
- Smooth transitions (200-300ms)
- Celebration animations for completions
- Progress bar fills
- Badge unlock animations
- Loading states for images

## Technical Requirements

### Performance
- Fast initial load (< 3 seconds)
- Lazy load images
- Optimize component images
- Progressive Web App (PWA) capabilities for offline use

### Browser Support
- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
- Mobile browsers (iOS Safari, Chrome Mobile)
- **WebAssembly support required** (all modern browsers)

### Responsive Design
- Mobile: 320px - 767px
- Tablet: 768px - 1023px
- Desktop: 1024px+
- Touch-friendly on all devices

### Data Persistence
- LocalStorage for all user data
- Auto-save after every action (level completion, quiz answer, etc.)
- Backup creation on significant milestones
- **Data Loss Warning**: If localStorage is cleared, all progress is lost
- **Recovery Mechanism**: Allow users to manually set starting level
- **Data Structure Documentation**: Clear comments in code explaining data format
- **No Backend**: Completely client-side application

### Browser Storage Strategy
```csharp
// LocalStorageService.cs - Service for managing localStorage
public class LocalStorageService
{
    private readonly ILocalStorageService _localStorage;
    private const string STORAGE_KEY = "circuitadventures-user";
    private const string BACKUP_KEY = "circuitadventures-backup";

    public async Task SaveUserDataAsync(UserData data)
    {
        await _localStorage.SetItemAsync(STORAGE_KEY, data);
        // Also save backup
        await _localStorage.SetItemAsync(BACKUP_KEY, data);
    }

    public async Task<UserData> LoadUserDataAsync()
    {
        return await _localStorage.GetItemAsync<UserData>(STORAGE_KEY);
    }

    // Recovery from backup
    public async Task<UserData> RecoverFromBackupAsync()
    {
        return await _localStorage.GetItemAsync<UserData>(BACKUP_KEY);
    }

    public async Task ClearDataAsync()
    {
        await _localStorage.RemoveItemAsync(STORAGE_KEY);
        await _localStorage.RemoveItemAsync(BACKUP_KEY);
    }
}
```

### Security
- No external API calls (initially)
- Input sanitization for notes
- Safe external links (open in new tab)

## Future Enhancements (Phase 2)
- Video tutorials for complex levels
- Community sharing of projects
- Parent/child accounts with Azure AD B2C
- Multiple profiles
- Print project worksheets
- Augmented reality circuit visualization (via Blazor + AR.js)
- Native mobile app (using .NET MAUI)
- Offline mode improvements
- Translation to other languages using resource files
- Audio instructions

## Development Approach

### Phase 1: MVP (Minimum Viable Product)
1. Create Blazor WebAssembly project (.NET 8)
2. Install NuGet packages (Blazored.LocalStorage, MudBlazor optional)
3. Implement onboarding flow (WelcomeScreen, NameEntry, LevelSelector components)
4. Set up LocalStorageService and dependency injection
5. Create basic routing and layout with user name display
6. Implement first 20 levels with full content and diagrams
7. Build completion flow with quiz (modal component)
8. Implement "Go to Next Level" functionality
9. Create certificate system with first 2 certificates (10, 20 levels)
10. Build certificate viewing and download feature
11. Basic progress tracking display
12. Level locking/unlocking mechanism
13. Interactive diagram viewer component with zoom/pan
14. Simple progress page with certificate gallery
15. Configure Program.cs with all services

### Phase 2: Complete Content
1. Add all 100 levels with detailed instructions
2. Create all circuit diagrams (SVG format)
3. Create component illustrations library
4. Add quiz questions for all levels
5. Implement challenge modes
6. Design all 10 certificates with unique styles
7. Add educational content sections

### Phase 3: Polish & Gamification
1. Add all badges and achievements
2. Implement streak system with calendar view
3. Add animations and celebrations using CSS animations
4. Improve diagram interactivity
5. Add certificate printing functionality
6. Certificate download in multiple formats (PNG, PDF via JSInterop)
7. Improve UI/UX based on testing
8. Add sound effects (optional, toggleable) via JSInterop

### Phase 4: Enhancement
1. PWA capabilities for offline use (service worker)
2. Data export/import for backup (JSON download/upload)
3. Additional parent dashboard features
4. Advanced diagram features (animations, overlays)
5. Text-to-speech for instructions via Web Speech API
6. Multiple user profiles (future consideration)
7. Certificate sharing to social media

## Content Creation Guidelines

### For Each Level, Create:
1. **Title**: Fun, descriptive name (3-5 words)
2. **Clear Learning Objective**: What will they learn?
3. **Component List**: Exact quantities needed with illustrations
4. **Safety Notes**: Age-appropriate warnings
5. **Step-by-Step Instructions**: 
   - 3-8 steps
   - Clear, simple language
   - One action per step
   - Numbered steps
6. **Circuit Diagrams** (SVG format):
   - **Main circuit diagram**: Complete overview
   - **Step diagrams**: One for each instruction step
   - **Component close-ups**: Detail views where needed
   - **Multiple perspectives**: Top view, side view, 3D view as needed
   - Features:
     - Simple, clear lines
     - Color-coded wires (red/black/other colors)
     - Labeled components with pointer lines
     - Connection points clearly marked
     - Numbered to match instruction steps
     - Grid background for breadboard layouts
7. **Educational Content**:
   - Why does this work? (simple science explanation)
   - Real-world applications
   - Fun fact
8. **Quiz Question**: Simple multiple choice
9. **Challenge Mode**: Optional harder version
10. **Tips and Tricks**: Helpful hints for success

### Language Guidelines
- Use simple, active voice
- Short sentences
- Avoid technical jargon (or explain it)
- Encouraging tone
- Use "you" to make it personal
- Include humor where appropriate
- Safety warnings in clear, serious tone

## Testing Requirements
- Test with actual 8-year-old users
- Parent feedback on safety and clarity
- Check all projects can be built with available components
- Verify time estimates
- Test on multiple devices
- Accessibility audit

## Success Metrics
- User completes at least 10 levels
- Average session time > 15 minutes
- User returns to app multiple times
- Positive feedback from child and parent
- All projects successfully buildable
- No safety incidents

## Getting Started
1. Create new Blazor WebAssembly project (.NET 8 or latest)
   ```bash
   dotnet new blazorwasm -o CircuitAdventures.Client
   ```
2. Install NuGet packages
   ```bash
   dotnet add package Blazored.LocalStorage
   dotnet add package MudBlazor (optional for rich UI components)
   ```
3. Set up basic folder structure (Pages, Components, Services, Models, Data)
4. Create model classes (UserData, Level, Certificate, etc.)
5. Build onboarding components (WelcomeScreen, NameEntry, LevelSelector)
6. Implement LocalStorageService and register in Program.cs
7. Build layout components with user greeting
8. Implement routing with navigation manager
9. Create level data classes
10. **Generate SVG component illustrations**
11. Build certificate templates (all 10 designs as Razor components)
12. Create certificate generation and download services
13. Build first 5 levels completely with diagrams
14. Implement completion flow with quiz and "Go to Next" button
15. Create certificate modal and gallery components
16. Create interactive diagram viewer component
17. Implement progress tracking service
18. Configure dependency injection in Program.cs
19. Test onboarding and level progression flow
20. Test certificate generation and download
21. Test data persistence and recovery
22. Iterate based on testing

---

## Critical User Flow

### First Visit
1. User opens app → Welcome screen
2. Enter name → Stored in localStorage
3. "Is this your first time?" → Choose starting level (default: 1)
4. → Home page with greeting

### Level Completion Flow
1. User on Level Detail page
2. Follows instructions and builds project
3. Clicks **"I've Built This!"** button
4. Quiz question modal appears
5. Answer quiz → Stars awarded (1-3)
6. Progress saved to localStorage
7. If milestone (10, 20, 30, etc.):
   - Certificate modal appears
   - Certificate added to collection
   - Download option available
8. **"Go to Next Level"** button appears
9. Click → Navigate to next level
10. Next level unlocked automatically

### Certificate Milestones
- Every 10 levels completed → New certificate
- Certificate automatically generated with user name
- Stored in localStorage
- Can be viewed/downloaded anytime from Progress page
- Each certificate has unique inspiring name and design
- All certificates issued by "CircuitAdventures"

---

## Diagram Generation Guidelines

### Component Illustrations (SVG)
Each component should have a standard illustration showing:
- Clear, recognizable shape
- Proper proportions
- Terminal/connection points clearly marked
- Labels for polarity (+ / -)
- Consistent style across all components
- Size reference (if needed)

**Component List to Illustrate:**
- LED (with long/short leg indicators)
- Battery (AA, with +/- terminals)
- Motor (with red/black wire terminals)
- Solar panel (with +/- wires)
- Fan blade
- Dynamo/hand-crank generator
- Jumper wires (male-male, various colors)
- Breadboard (with labeled rows/columns)
- Capacitor (with polarity marking)
- Resistor (with color band pattern)
- Switch (on/off positions)
- Small bulb (with base)

### Circuit Diagram Standards
1. **Color Coding**:
   - Red wires: Positive power
   - Black wires: Negative/ground
   - Yellow: Signal/control
   - Blue: Alternative signal
   - Green: Earth/chassis ground

2. **Layout Style**:
   - Clean, orthogonal wire routing
   - Avoid diagonal wires unless necessary
   - Component spacing for clarity
   - Grid-aligned for breadboard layouts

3. **Labeling**:
   - Component names (e.g., "Motor", "LED")
   - Values where relevant (e.g., "9V Battery")
   - Terminal names (+ / - / Signal)
   - Step numbers in circles

4. **Views to Provide**:
   - **Schematic**: Electronic symbol representation
   - **Physical Layout**: How it actually looks assembled
   - **Breadboard View**: Top-down with wire routing
   - **3D View** (optional): Isometric for complex builds

### Interactive Features
- Clickable components to show detailed info
- Step-by-step reveal mode
- Wire path highlighting
- Zoom hotspots for detailed areas
- Animation of electricity flow (optional)

## Deployment

### GitHub Pages Deployment

This project includes automated GitHub Pages deployment via GitHub Actions.

**Quick Setup:**
1. Push your code to GitHub
2. Go to **Settings** → **Pages**
3. Set source to **GitHub Actions**
4. Go to **Settings** → **Actions** → **General**
5. Enable **Read and write permissions**
6. Push to `main` branch - deployment happens automatically!

Your site will be live at: `https://<username>.github.io/<repository-name>/`

**For detailed deployment instructions, see [DEPLOYMENT.md](DEPLOYMENT.md)**

### Local Development

```bash
# Clone the repository
git clone <repository-url>
cd circuitadventures

# Restore dependencies
dotnet restore

# Run the application
dotnet run --project CircuitAdventures.Client

# Open http://localhost:5190 in your browser
```

## Notes for Claude Code
- Start with a clean Blazor WebAssembly project template
- Use .NET 8 or latest LTS version
- Follow Blazor best practices and conventions
- Use dependency injection for all services
- Implement responsive design from the start using Bootstrap 5
- Make components reusable (Razor components)
- Add helpful XML comments for documentation
- Follow C# coding conventions
- Ensure accessibility standards (ARIA labels, semantic HTML)
- Create a fun, engaging UI suitable for children
- Safety should be paramount in all instructions
- **Diagrams should be SVG files or inline SVG in Razor components**
- **Diagram viewer should support zoom, pan, and interactive elements**
- **Consider using JSInterop for advanced diagram interactions**
- **All visual content should be vector-based (SVG) for scalability**
- **Implement robust localStorage operations with error handling via Blazored.LocalStorage**
- **Always save user progress immediately after any action using async/await**
- **Create beautiful, printable certificate designs as Razor components**
- **Use JSInterop for certificate download as PNG/PDF**
- **Implement clear user feedback for all actions (toast notifications, animations)**
- **"Go to Next Level" button should be the primary CTA after completion**
- **Test localStorage persistence thoroughly**
- **Handle edge cases: localStorage full, data corruption, null checks**
- **Make onboarding flow smooth and intuitive for 8-year-olds**
- **Register all services in Program.cs with proper lifetimes (Singleton for data services)**
- **Use Bootstrap classes for styling, supplement with custom CSS**
- **Optimize for WebAssembly bundle size**