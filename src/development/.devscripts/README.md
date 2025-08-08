# Test Runner Script

This directory contains scripts for running tests within the project.

## `runtests.sh`

### Description

This is a bash script that automates the execution of all unit and integration tests across the `.webapi.tests.unit`, `.webapi.tests.t1`, and `.webapi.tests.t2` projects.

### How to Use

1.  **Navigate to the project root directory:**

    ```bash
    cd /mnt/c/dev/WhoAMI
    ```

2.  **Execute the script:**

    ```bash
    ./src/development/.devscripts/runtests.sh
    ```

### Expected Output

The script will output the results of each test run to the console, including:

*   Messages indicating which test project is currently being run.
*   Detailed test results from `dotnet test`, showing passed, failed, and skipped tests.
*   Any warnings or errors encountered during the test execution (though ideally, there should be none after recent fixes).
