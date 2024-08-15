# APL - Hospital Management System (HMS)

Medical management system for [Ann Prepare Lavni (APL)](https://annpreparelavni.org) 

This will be a software developed especifically for APL's mission trips and clinics, and its current needs. It will not be a general hospital/clinic management system.

Frameworks: Angular with ASP.NET Core

Constraints:
  - The software cannot rely on internet, they can sometimes stay days with no internet or with extremely low speeds;
  - It must be very sturdy and reliable, with a lot of redundancy, there is no option of remote updates (except on rare cases) as of now;
  - Since the users won't be very technical, it must be easy to maintain with almost no technical background.

The current idea/goal is to have the system running locally in a raspberry pi, with a local DB. Since internet is not reliable locally, the software will schedule to check internet access frequently and always try to send the current local data to the cloud, to have an independent copy online.

Functionality (must-haves):
  - Must print patient forms with fields ready to also be handwritten after printed
  - Iternationalization (English, Haitian Kreyol and Spanish, currently)
      - Language selection after sign-in (if the account logged-in doesn't already have one selected)
  - One user starts the patient records and another one can finish it (separate the proccess into several parts)

Users:
  - Doctor
  - Nurse
  - Check-in

Config:
  - Use SQLite for local config

### Current Triage Patient Form design idea (framework independent for now)
![image](https://github.com/RicardoDeGenova/hospital-management-system/assets/79471515/52eaf41f-92bb-49b6-9057-ba60f463a469)


### Proposed Views: Patient Registration View (framework independent for now)
![image](https://github.com/RicardoDeGenova/hospital-management-system/blob/main/Resources/Patient%20Registration%20View.png)


### Proposed Views: Triage And Assesment View (framework independent for now)
![image](https://github.com/RicardoDeGenova/hospital-management-system/blob/main/Resources/Triage%20And%20Assesment%20View.png)


Have meds separed by colored sections
Create sections for each type of med that the user filters, example: 
  - Section1 > Section2 > Med

Inicial meds, should already load db with it:

  - Acetaminophen + caffeine
  - Acetaminophen drops
  - Acetaminophen susp
  - Acetominophen 500mg
  - Acyclovir 400mg
  - Acyclovir cream
  - Albendazole 400mg
  - Albendazole 400mg susp
  - Amitryptiline 25mg 
  - Amlodipine 10mg
  - Amoxcillin susp
  - Amoxicillin + Clauvanate susp
  - Amoxicillin 500mg
  - Antigripal
  - Aspirin 325mg
  - Aspirin 81mg
  - Atenolol 50mg
  - Azithromycin 500mg
  - Bactrim cap Bactrim susp 
  - B Complex
  - Calcium Carbonate 
  - Carvedilol 12.5mg
  - Cephelaxin 250 susp 
  - Cephelaxin 500mg 
  - Ceterizine 10mg
  - Ciprofloxacin 500mg
  - Citicolina
  - Clotrimazol creme 
  - Clotrimazol ovules 500mg
  - Dexamethasone 8mg 
  - Diclofenac 50mg
  - Diphenhydramine 10mg/ml 
  - Diphenhydramine 25mg 
  - Diphenhydramine drops 
  - Doxycycline 100mg 
  - Enalapril 10mg
  - Enalapril 20mg
  - Ensure liquid
  - Ensure powder
  - Fluconazole 150mg
  - Fluoride Varnish
  - Furosemide 40mg
  - Furosemide 40mg IV Gentamicina cream Gencloben cream
  - Griseofulvin 500mg
  - Griseofulvin suspension 
  - Hydrocortisone cream 
  - Iron Sulfate
  - IV Fluids Dextrose
  - IV Fluids Normal Saline
  - Ivermectin 6mg
  - Ibuprofen 400mg
  - Ibuprofen 600mg
  - Ibuprofen 800mg
  - Jabon de azufre Lasurtin
  - Levofloxacin 500mg
  - Lidocaine 2% 
  - Lisinopril 10mg 
  - Loratadine 10mg
  - Oral hydration salts
  - Otoful gotas 
  - Meloxicam 
  - Metformin 500mg 
  - Metformin 850mg
  - Metoclopramide 10mg
  - Metronidazole + Nistatina
  - Metronidazole 500mg
  - Montelukast 10mg
  - Multivitamins
  - Multivitamins drops
  - Omeprazole 20mg
  - Prednisone 20mg
  - Permetherin lotion
  - Permetherin cream
  - Permetherin shampoo
  - Prenatal vitamins 
  - Salbutamol 5mg/ml
  - Spirinolactone 25mg 
  - Tamsulosin
  - Vaseline


## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::8.0.5
```

## Test

The solution contains unit, integration, functional, and acceptance tests.

To run the unit, integration, and functional tests (excluding acceptance tests):
```bash
dotnet test --filter "FullyQualifiedName!~AcceptanceTests"
```

To run the acceptance tests, first start the application:

```bash
cd .\src\Web\
dotnet run
```

Then, in a new console, run the tests:
```bash
cd .\src\Web\
dotnet test
```
