# Security Convention

## 1. Authentication and Authorization
- Implement robust authentication for users
- Use well-defined roles and permissions to control access to functionalities and data  
- Brute force attack protection: after 5 attempts, the system must reject the connection  

## 2. Data Protection
- Encrypt sensitive data in the database (passwords, personal information)  
- Do not store sensitive information in logs or in the frontend  

## 3. Database
- Limit permissions of the database account (principle of least privilege)  
- Perform periodic backups and test restorations  

## 4. Input Validation
- Validate and sanitize all user inputs (both frontend and backend)  

## 5. Dependency Management
- Keep libraries and frameworks up to date  
- Review known vulnerabilities in dependencies  
- Avoid using outdated or unmaintained packages  

## 6. Logging and Monitoring
- Record security events (failed login attempts, unauthorized access)  
- Do not log sensitive information (passwords, tokens)  
- Periodically review logs to detect suspicious activities  

## 7. Secure Deployment
- Use separate environments (development, testing, production)  
- Secure the deployment infrastructure (configure firewall, limit open ports)  

## 8. Awareness and Training
- The entire team must be familiar with best security practices  
- Conduct code reviews with a focus on security  
- Document security decisions in the design phase  
- Use secure versions of the required software  

## 9. Vulnerability Analysis and Testing

### STRIDE
A conceptual tool to help identify vulnerabilities. Each epic is analyzed using the following axes:  
- **Spoofing**: Identity impersonation  
- **Tampering**: Data alteration  
- **Repudiation**: A user performs an action and later denies having done it  
- **Information Disclosure**: Exposure of sensitive data or system information  
- **Denial of Service (DoS)**: Resource exhaustion (CPU, memory, connections, application limits)  
- **Elevation of Privilege**: A regular user gains administrative privileges or access to restricted capabilities  

### Security Regression Tests
White-box tests that verify security requirements. Example tests:  
- Authorization  
- Input validation  
- SQL injection  