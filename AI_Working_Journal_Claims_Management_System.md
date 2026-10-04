# AI Working Journal

This is a running log of how AI was used during the development of the Claims Management System.

The purpose is to record what I asked AI, what I accepted, what I challenged, and what I changed or implemented myself.

---

## 1. Project Structure & Architecture

### Asked AI

I asked AI for suggestions on how to structure the Claims API and separate controllers, services, repositories, DTOs and persistence concerns.

### Accepted

I kept the modular structure around the Claims feature because it made the project easier to navigate and maintain.

### Challenged / Changed

I did not blindly follow the proposed folder structure. I kept the structure aligned with my existing implementation instead of introducing unnecessary layers just to follow a textbook Clean Architecture structure.

### Reason

The goal was to keep the architecture appropriate for the scope of the project rather than over-engineering it.

---

## 2. Claim Status Workflow

### Asked AI

I asked AI how the claim workflow should handle statuses such as Submitted, UnderReview, PendingAssessment, Approved and Rejected.

### Accepted

I used backend validation to prevent operations from being performed when the claim is in an invalid state.

For example, assessment should only proceed when the claim is in an appropriate status.

### Challenged / Changed

I changed the condition when I realised that `UnderReview` and `PendingAssessment` represent different stages of the workflow.

### Reason

The status transition needs to reflect the actual business process instead of simply allowing multiple statuses without a clear workflow.

---

## 3. Debugging Angular Authentication

### Problem

The development login endpoint worked in Postman, but Angular requests were blocked or unauthorized.

### Asked AI

I asked AI to help identify why the Angular request was different from the working Postman request.

### Investigation

I checked the browser Network tab and compared the actual request headers.

The issue was related to how the token response was being handled.

The response was:

```json
{
  "token": "eyJ..."
}
```

but the Authorization header must contain only the JWT:

```text
Authorization: Bearer eyJ...
```

### Accepted

I updated the Angular authentication flow so that the actual token string is stored and used in the Authorization header.

### Reason

I verified the solution using the actual browser request instead of accepting the AI suggestion without checking it.

---

## 4. Policy Integration

### Asked AI

I asked AI how to display the Claimant's policies and allow the user to select a policy when submitting a claim.

### Accepted

The Claim page loads the Claimant's policies and uses the selected `policyId` when creating a claim.

### Debugging

Postman returned the expected policies, but Angular initially did not display them.

I checked:

1. API response
2. Browser Network request
3. Component state
4. Template binding

### Result

The backend was returning the correct data. The issue was in the Angular component/template binding rather than the API.

### Reason

This reinforced the importance of checking each layer before changing the backend.

---

## 5. Claim Submission

### Asked AI

I asked AI to help structure the Claimant claim submission flow.

### Accepted

The frontend collects:

- Claim number
- Policy
- Incident date
- Incident description
- Estimated liability

The backend then processes the request using the authenticated Claimant identity.

### Reason

The policy relationship is validated by the backend rather than relying only on frontend selection.

---

## 6. Dashboard

### Asked AI

I asked AI how to structure the Claims Officer dashboard metrics.

### Accepted

The dashboard exposes information such as:

- Total claims
- Claims under review
- Rejected claims
- Estimated loss
- Settlement amount
- Officer workload
- Recent claims

### Changed

I aligned the calculations with the actual claim and settlement data in the database instead of using static or frontend-calculated values.

### Reason

Operational metrics should be derived from backend data so that the dashboard remains consistent with the system of record.

---

## 7. What I Learned From Using AI

The main value of AI during this project was not simply generating code.

I used it mainly to:

- challenge implementation decisions
- identify possible causes of errors
- compare implementation approaches
- explain framework behaviour
- speed up repetitive development work

However, I still validated the suggestions through:

- compiler errors
- API responses
- Postman
- browser Network tools
- database results
- actual application behaviour

When the AI suggestion did not match the project requirements or existing implementation, I changed or rejected it.

---

## 8. Overall Reflection

AI helped reduce the time spent searching for solutions and explaining unfamiliar behaviour, but it did not replace the development process.

The most useful workflow was:

**Ask → Implement → Test → Observe → Challenge → Adjust → Retest**

This allowed me to use AI while still making the final technical decisions based on the actual behaviour of the system.
