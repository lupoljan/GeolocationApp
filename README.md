# GeolocationApp

## 1. Functional Requirements

### Core Features

#### Geolocation Fetching
- Input IP address or URL
- Fetch geolocation data from ipstack API
- Display: Country, City, Region, Latitude, Longitude, ISP, etc.

#### Database Operations
- Add fetched data to SQLite DB
- Delete records
- Display stored records

#### User Interface
- WPF with Material Design
- Input field with validation
- Buttons: Fetch, Save, Delete
- DataGrid for stored records
- Status/Error messages

## 2. Technical Specifications

- **Framework**: .NET 7/8
- **UI**: WPF + Material Design
- **Database**: SQLite + Entity Framework Core
- **API**: ipstack (HTTP REST)
- **MVVM**: Community Toolkit (ObservableObject, RelayCommand)
- **Packages**:
  - MaterialDesignThemes
  - Microsoft.EntityFrameworkCore.Sqlite
  - Newtonsoft.Json

## 3. User Workflow

1. Enter IP/URL → Click Fetch → Display results
2. Click Save → Store in database
3. Select record → Click Delete → Remove from UI and database

## 4. API Integration

- **Endpoint**: `http://api.ipstack.com/{input}?access_key={API_KEY}`
- **Validation**: Check if input is a valid IP/URL before API call

## 5. Assumptions & Dependencies

- A valid ipstack API key is provided
- .NET 8 runtime is installed
- No user authentication required
- Single-user local database
