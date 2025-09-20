# QA Report – UI and API Endpoints Issues

---

## Note:

All UI-related issues were tested using the following login page:  
🔗 https://wmxrwq14uc.execute-api.us-east-1.amazonaws.com/Prod/Account/LogIn

**For security reasons, credentials are not exposed in this report.**

---

## Test Environment

- **Browsers Used:** Google Chrome (latest version), Microsoft Edge (latest version)  
- **API Validation:** Manual testing using Postman

---

## QA Release Recommendation



> ## **NO GO** – Deployment is not approved due to the presence of a critical **Showstopper** defect.
>
> **Blocking Issue:**  

- *First Name and Last Name are reversed in the UI after adding an employee (UI–API integration defect).*

This defect directly impacts data integrity and user trust, and must be resolved before deployment to production.

---

## UI–API Issues

### [Showstopper] First Name and Last Name are switched after adding an employee
- **Type:** UI–API  
- **Severity:** Showstopper  
- **Description:** After adding an employee, the UI renders First Name and Last Name swapped, even though the API POST is correct.  
- **Expected:** After adding an employee, the table should display the `First Name` in the First Name column and the `Last Name` in the Last Name column, matching the values entered in the form.  
- **Actual:** The values for `First Name` and `Last Name` are swapped in the employees table. The `First Name` column shows the last name, and the `Last Name` column shows the first name  
- **Steps to Reproduce:**  
  1. Log in with valid credentials.  
  2. Click the Add Employee button.  
  3. In the First Name field, enter `John`.  
  4. In the Last Name field, enter `Smith`.  
  5. Fill in the remaining required fields with valid data (e.g., `Dependents = 2`).  
  6. Click Add to save the employee.  
  7. Observe the employees table:  
     - The First Name column shows `Smith`.  
     - The Last Name column shows `John`.  
  - **Notes:**  
    - The swap happens in the UI rendering, the API Post is working as expected.

---

### [High] Dependents value higher than 32 is accepted
- **Type:** UI–API  
- **Severity:** High  
- **Description:** The system accepts dependents values above the schema’s maximum of 32.  
- **Expected:** Maximum allowed is 32 according to the schema  
- **Actual:** Any value higher than `32` is accepted  
- **Steps to Reproduce:**  
  1. Go to the login page and log in with valid credentials.  
  2. Click the "Add Employee" button.  
  3. In the First Name field, type a valid name (e.g., `Berenice`).  
  4. In the Last Name field, type a valid name (e.g., `Ponce`).  
  5. In the Dependents field, type `33`.  
  6. Click the "Add" button.  
  7. Observe that the employee is created even though the dependents value is above the allowed limit (32).

---

### [Medium] Inconsistent Spelling for "Dependents" Field
- **Type:** UI / API  
- **Severity:** Medium  
- **Description:** The UI uses US English (`Dependents`) while the API schema uses British English (`dependants`).  
- **Expected:** The spelling of the dependents field should be consistent across both the UI and the API schema, using the same English variant (either US: `Dependents` or UK: `Dependants`).  
- **Actual:** The UI (column header and Add Employee form) uses `Dependents` (US English), while the API schema property name is `dependants` (British English).  
- **Steps to Reproduce:**  
  1. Open the Employees table in the UI.  
  2. Observe the column header for the dependents count, it reads "Dependents".  
  3. Click Add Employee and check the label for the dependents input, it also reads "Dependents".  
  4. Inspect the API schema for the Employee object (or GET `/Employees` response).  
  5. Note that the property name is "dependants".

---

## UI Issues

### [High] Dependents field accepts characters, float numbers, and spaces after an integer
- **Type:** UI  
- **Severity:** High  
- **Description:** The Dependents input accepts invalid formats (decimals, letters, trailing spaces) and still creates the employee.  
- **Expected:** Only integer numbers should be allowed, with no extra characters, decimal points, or spaces after the number  
- **Actual:** The field accepts extra characters, decimal points, or spaces after a valid integer (e.g., `2.5`, `3abc`, `4 `)  
- **Steps to Reproduce:**  
  1. Go to the login page and log in with valid credentials.  
  2. Click the "Add Employee" button. 
  3. In the First Name field, type a valid name (i.e., `John`).  
  4. In the Last Name field, type a valid name (i.e., `Smith`).  
  5. In the Dependents field, type a valid integer followed by a decimal, letters, or spaces (e.g., `2.5`, `3abc`, `4 `).  
  6. Click the "Add" button.  
  7. Observe that the employee is created even though the dependents value contains invalid characters after the integer.

---

### [High] No error message when dependents value is invalid
- **Type:** UI  
- **Severity:** High  
- **Description:** Submitting an invalid dependents value does not show any validation error message.  
- **Expected:** Show error message after click on Add button  
- **Actual:** No message is shown  
- **Steps to Reproduce:**  
  1. Log in with valid credentials.  
  2. Click the "Add Employee" button.  
  3. In the First Name field, type a valid name (e.g., `Carlos`).  
  4. In the Last Name field, type a valid name (e.g., `Perez`).  
  5. In the Dependents field, type `-1` or `abc`.  
  6. Click the "Add" button.  
  7. Observe that no error message is displayed.

---

### [Medium] First Name and Last Name accept numbers and special characters

- **Type:** UI  
- **Severity:** High
- **Description:** Name fields allow non-letter characters (numbers, symbols), leading to invalid data.  
- **Expected:** Only letters should be allowed in both First name and Last Name  
- **Actual:** Fields accept values like `123Eduardo` or `@Smith!` `@12%!`  
- **Steps to Reproduce:**  
  1. Go to the login page and log in with valid credentials.  
  2. Click the "Add Employee" button.  
  3. In the First Name field, type `123Eduardo`.  
  4. In the Last Name field, type `@Smith!`.  
  5. In the Dependents field, type a valid number (e.g., `2`).  
  6. Click the "Add" button.  
  7. Observe that the employee is created with invalid name values.

---

### [High] Sometimes session ends right after login (sporadic)

- **Type:** UI  
- **Severity:** Medium
- **Description:** Sporadically, the session terminates a few seconds after login, preventing any operations.  
- **Expected:** Session should stay active  
- **Actual:** After login, session ends in a few seconds  
- **Steps to Reproduce:**  
  - This happens sometimes and is not consistent.  
  - It happens from time to time

---

### [Medium] No message when session ends
- **Type:** UI  
- **Severity:** Medium  
- **Description:** When the session expires, the UI does not notify the user, causing actions to fail silently.  
- **Expected:** Show message when session expires  
- **Actual:** No message is shown  
- **Steps to Reproduce:**  
  1. Log in with valid credentials.  
  2. Wait without doing anything until the session expires (It takes around 10 minutes).  
  3. Try to add or edit an employee.  
  4. Observe that no message is shown and the action fails (Add, Edit, Delete).

---

### [Low] Some ID strings break table layout
- **Type:** UI  
- **Severity:** Low  
- **Description:** Some IDs wrap to a second line, affecting readability of the table.  
- **Expected:** IDs should be shown in one line  
- **Actual:** Some IDs take two lines  
- **Steps to Reproduce:**  
  1. Log in with valid credentials.  
  2. Add 5 employees with valid data.  
  3. Look at the ID column in the table.  
  4. Observe that some IDs wrap to a second line.  
  - NOTE: Although the length for all of the ids is the same, some ids takes 2 lines.

---

### [Low] Edit window says "Add Employee" instead of "Edit Employee"
- **Type:** UI  
- **Severity:** Low  
- **Description:** The edit dialog title shows “Add Employee” instead of “Edit Employee,” which can confuse users.  
- **Expected:** Title should say "Edit Employee"  
- **Actual:** Title says "Add Employee"  
- **Steps to Reproduce:**  
  1. Log in with valid credentials.  
  2. Add an employee with valid data.  
  3. In the table, click the Edit option for that employee.  
  4. Observe that the window title says "Add Employee" instead of "Edit Employee".

---

## API Issues

### [High] PUT updates expiration but GET shows the old value
- **Type:** API  
- **Severity:** High  
- **Description:** After updating expiration via PUT, subsequent GET returns the previous expiration value.  
- **Expected:** GET should show new expiration  
- **Actual:** GET shows old expiration date  
- **Steps to Reproduce:**  
  1. Create an employee with valid data.  
  2. Send a GET request for that employee and take note of the Expiration date  
  2. Send a PUT request to `/api/Employees` with a new expiration date.  
  3. Send a GET request for that employee.  
  4. See that the expiration date is still the old one (step 2).

---

### [High] DELETE returns 405 for malformed or bad ID
- **Type:** API  
- **Severity:** High  
- **Description:** DELETE requests with malformed IDs return 405 instead of the expected 400.  
- **Expected:** Return 400 for malformed or bad ID  
- **Actual:** Returns 405  
- **Steps to Reproduce:**  
  1. Send a DELETE request to `/api/Employees/abc123`.  
  2. See that the response is 405 instead of 400.

---

### [High] DELETE returns 200 for unknown ID
- **Type:** API  
- **Severity:** High  
- **Description:** Deleting a non-existent ID returns success (200) instead of not found (404).  
- **Expected:** Return 404 for unknown or not found ID  
- **Actual:** Returns 200  
- **Steps to Reproduce:**  
  1. Send a DELETE request with a valid but non-existent ID.  
  2. See that the response is 200 instead of 404.

---

### [Medium] POST returns 200 creating a new employee

- **Type:** API  
- **Severity:** Medium  
- **Description:** Creating a valid employee, request returns success (200) instead of not found (201).  
- **Expected:** Return 201 for Created
- **Actual:** Returns 200  
- **Steps to Reproduce:**  
  1. Send a POST request with a valid body to create a new employee
  2. See that the response is 200 instead of 201.

---

### [High] POST accepts expiration in the past, but GET shows default
- **Type:** API  
- **Severity:** High  
- **Description:** API accepts past expiration dates on POST but changes is not reflected in database.  
- **Expected:** Show same expiration from POST in GET  
- **Actual:** GET shows default expiration  (original)
- **Steps to Reproduce:**  
  1. Send a POST request to `/api/Employees` with valid data and an expiration date in the past.  
  2. Send a GET request for that employee.  
  3. See that the expiration date is changed to the default value.

---

### [High] POST accepts request without username
- **Type:** API  
- **Severity:** High  
- **Description:** API allows creating employees without the required `username` field.  
- **Expected:** Return 400  
- **Actual:** Returns 200  
- **Steps to Reproduce:**  
  1. Send a POST request to `/api/Employees` with valid firstName and lastName but without username.  
  2. See that the Employee is added without errors (200)

---

### [Medium] POST accepts unexpected "additional" parameters and returns 200
- **Type:** API  
- **Severity:** Medium  
- **Description:** API accepts fields not defined in the schema (e.g., `address`) and ignores them instead of rejecting the request.  
- **Expected:** The API should reject any unknown or unexpected parameters in the request body and return 400 Bad Request.  
- **Actual:** When sending a POST request with an extra field called `address`, the API still returns 200 OK and creates the employee. The `additionals` field is ignored in the stored record and is not returned in the response.  
- **Steps to Reproduce:**  
  1. Prepare a POST request to `/api/Employees` with all required valid fields:  
  2. Add an extra field in the JSON body, for example:  
     {  
       "firstName": "John",  
       "lastName": "Smith",  
       "dependents": 2,  
       "address": "Fake Address"  
     }  
  3. Send the request.  
  4. Observe that the API responds with 200 OK and creates the employee, instead of rejecting the request with 400 Bad Request.

---

### [Low] PUT response removes expiration if it is not included in the body
- **Type:** API  
- **Severity:** Low  
- **Description:** When expiration is omitted in a PUT request, it is missing from the response instead of being preserved.  
- **Expected:** Expiration should be shown in the response  
- **Actual:** Expiration is removed from the response  
- **Steps to Reproduce:**  
  1. Create an employee with valid data, including an expiration date.  
  2. Send a PUT request to `/api/Employees` with updated fields but without the expiration field.  
  3. Check the response and see that the expiration date is missing.

---

## Enhancements

### [Low] Sort employees by Last Name
- **Type:** UI  
- **Severity:** Low  
- **Description:** Sort table by Last Name instead of ID as it it hard to find the Employes

---

### [Low] Add filter at the top of each column
- **Type:** UI  
- **Severity:** Low  
- **Description:** Add filters to make the search easier

---

### [Low] Show tooltip when mouse is over Edit or Delete options
- **Type:** UI  
- **Severity:** Low  
- **Description:** Show message like "Edit Employee" or "Delete Employee" when mouse is over the options