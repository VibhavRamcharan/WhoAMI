# AccountAPI.Tests.T3 - Performance Testing with NBomber

This project is dedicated to performance testing the `AccountAPI` using [NBomber](https://nbomber.com/). It allows you to simulate various load conditions on the API endpoints to measure their performance characteristics.

## Project Setup

This project was set up using the following steps:

1.  **Create the project folder:**
    ```bash
    mkdir -p src/development/.webapi.tests.t3
    ```

2.  **Create a new .NET console application:**
    ```bash
    dotnet new console -n AccountAPI.Tests.T3 -o src/development/.webapi.tests.t3
    ```

3.  **Add the NBomber NuGet package:**
    ```bash
    dotnet add src/development/.webapi.tests.t3/AccountAPI.Tests.T3.csproj package NBomber
    ```

## How to Run Performance Tests

To run the performance tests, you need to ensure the `AccountAPI` is running first. The tests are configured to hit `http://localhost:8080`.

1.  **Start the AccountAPI (if not already running):**

    Navigate to the `src/development/.webapi` directory and run:
    ```bash
    dotnet run --project AccountAPI.csproj
    ```
    The API should be accessible at `http://localhost:8080`.

2.  **Navigate to the performance test project:**

    ```bash
    cd /mnt/c/dev/WhoAMI/src/development/.webapi.tests.t3
    ```

3.  **Run the NBomber tests:**

    ```bash
    dotnet run
    ```

### Current Scenario: `login_scenario`

The `Program.cs` file currently contains a basic scenario named `login_scenario`. This scenario performs the following:

*   **Target Endpoint:** `POST http://localhost:8080/Account/login`
*   **Payload:** Sends a JSON body with `username: "testuser"` and `password: "password"`.
*   **Load Simulation:** Keeps a constant load of 1 copy (virtual user) for 30 seconds.

### Interpreting Results

After running `dotnet run`, NBomber will output a summary of the test results to your console. Key metrics to look for include:

*   **OK/Fail Ratio:** Percentage of successful requests.
*   **RPS (Requests Per Second):** Throughput of the API.
*   **Latency (min, mean, max, p95, p99):** Response times in milliseconds.

### Next Steps / Customization

*   **Modify `Program.cs`:** You can edit `Program.cs` to:
    *   Change the target endpoint.
    *   Adjust the request payload.
    *   Define more complex load simulations (e.g., ramp-up, more copies, different durations).
    *   Add more scenarios to test different API endpoints or user flows.
*   **Add more scenarios:** Create additional scenarios to test other API functionalities.
*   **Distributed Testing:** For higher loads, NBomber supports distributed testing, which would involve running the tests from multiple machines.
