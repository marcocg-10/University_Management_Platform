# 📑 User Story & PBI Convention

## 🆔 Identification

- **ID:** `PREFIX-TEAM-###`  
  - **Prefix (Initials):** Represents the functionality (e.g., `LOG` = Log-in, `REG` = Registration, `USRMGMT` = User Management, `NOTIF` = Notifications).  
  - **Team ID:** Team code (e.g., `PIBDIS`, `Tilinos`, `Pozoles`, `DevOps`).  
  - **Number:** Sequential within the functionality.  

**Example:**  
`LOG-Tilinos-001` → Log-in, Group Tilinos, story #1.  
`USRMGMT-Pozoles-004` → User Management, Team Pozoles, story #4.  

## 📝 User Story Format

```plaintext
As a [type of user]
I want to [perform an action]
So that [specific goal or benefit]
```

**Example:**

```plaintext
As a user
I want to register on the website
So that I can access the website
```

## 🎯 Acceptance Criteria Format

- *Given* [Initial context]  
- *When* [Action performed]  
- *Then* [Expected outcome]  

**Example:**

```plaintext
Scenario: Successful Registration
Given that the user is on the registration page
When the user enters name, email, password and clicks "Register"
Then the system creates a new user account
And redirects the user to the login page with a confirmation message
```

📌 **Guidelines:**

- Include both positive and negative test cases.  
- Be specific and avoid ambiguity.  
- Always follow the *Given-When-Then* format.  

## ✅ Definition of Done (DoD)

1. Meets all acceptance criteria.  
2. Includes unit tests, passes and does not break existing tests.  
3. Follows the project’s coding conventions.  
4. Solves the associated User Story or PBI.  

## 📊 Estimation

- Use *Story Points* (Fibonacci sequence: 0, 0.5, 1, 2, 3, 5, 8, 13, 20, 100).  

## 🚦 Priority Levels

- **Highest** → Must be delivered immediately; blockers for other stories.  
- **High** → Business critical, should be prioritized in the current sprint.  
- **Medium** → Important but not urgent; can be postponed if necessary.  
- **Low** → Nice-to-have, adds value but not required in the short term.  
- **Lowest** → Optional, minimal impact if not implemented.  

## ⚠️ Risk Levels

- **High** → High uncertainty, technical/architectural complexity, or strong business risk.  
- **Normal** → Some challenges expected, but manageable within the sprint.  
- **Low** → Straightforward to implement, low uncertainty.  

## 📌 Status Options

- **New** → Just created, not yet reviewed.  
- **Not Started** → Reviewed, but work has not yet begun.  
- **Assigned** → A team or developer has been assigned to the story.  
- **In Progress** → Work is currently being done.  
- **Ready for Test** → Development is complete, awaiting QA validation.  
- **Done** → All acceptance criteria met, development completed, tested.  
- **Accepted** → Approved by PO; ready for release or already released.  
- **Canceled** → The story is no longer needed.  
- **Postponed** → Deferred to a future sprint/release.  
- **Blocked** → Cannot proceed due to external dependencies or issues.  

## 💬 PO Notes (Product Owner)

This section captures **key remarks or clarifications from the Product Owner previously discussed** regarding the story:  

- Business decisions.
- Scope clarifications.
- Technical considerations.
- Changes requested during grooming/refinement.

**Example Notes:**

- PO clarified that email validation must be real-time.
- PO mentioned that password recovery will not be included in the first version.

---

[↩️ Back to Technical Decisions README](./05-technical-decisions.md)
