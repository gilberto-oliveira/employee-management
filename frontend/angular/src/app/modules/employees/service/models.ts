
// Employee
export interface Employee {
    id: string;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    docNumber: string | null;
    phones: string[];
    managerId: string | null;
    manager: Employee | null;
    password: string | null;
    dateOfBirth: string; // DateTime
    role: string | null;
}

// EmployeeCreateRequest
export interface EmployeeCreateRequest {
    firstName: string | null;
    lastName: string | null;
    docNumber: string | null;
    email: string | null;
    phones: string[];
    password: string | null;
    role: string | null;
    dateOfBirth: string; // DateTime
    managerId: string | null;
}

// EmployeePaginatedCollection
export interface EmployeePaginatedCollection {
    page: number;
    limit: number;
    total: number;
    items: Employee[] | null;
}

// Operation
export interface Operation {
    path: string | null;
    op: string | null;
    value: any | null;
}

export enum EmployeeRole {
    Employee = "Employee",
    Director = "Director",
    Leader = "Leader",
}

export function convertEmployeeCreateRequestToUpdateOperations(
    employee: EmployeeCreateRequest
): Operation[] {
    const operations: Operation[] = [];
    Object.entries(employee)
        .filter(([key]) => key !== 'id' && key !== 'phones' && key !== 'managerId')
        .forEach(([key, value]) => {
            if (value !== null) {
                operations.push({
                    op: 'replace',
                    path: key,
                    value,
                });
            }
        });

    return operations;
}

