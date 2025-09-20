# Paylocity_QA_Automation

This repository contains a modular QA automation framework for testing both API and UI components of the Paylocity page challenge. It is built using **C#**, **.NET 8**, with **Playwright** for UI and RestSharp for **API testing**

---

## ⚙️ Prerequisites

Ensure the following are installed:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- Chrome or Edge browser (for UI tests)

---

## 🚀 How to Restore and Run Tests

### API Tests

```bash
cd Paylocity_QA_Automation/QA_Automation.API
dotnet restore
dotnet test
```

### UI Tests

```bash
cd Paylocity_QA_Automation/QA_Automation.UI
dotnet restore
playwright install 
dotnet test
```

