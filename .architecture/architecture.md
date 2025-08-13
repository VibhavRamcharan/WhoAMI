# WhoAMI MVP Architecture: Cost-Effective, Low-Latency Deployment

This document outlines a proposed architecture for the WhoAMI MVP, designed to provide a fast user experience across multiple South African locations (Durban, Johannesburg, Cape Town) while adhering to a strict budget of $300/month for total infrastructure.
Can 
## Core Principles

*   **Edge Performance:** Deploying Web API instances geographically closer to users to minimize network latency for API calls.
*   **Centralized Data:** Utilizing a single, robust database instance for data consistency and simplified management, accepting higher latency for database operations from remote Web APIs (which is acceptable for low initial user load).
*   **Cost Optimization:** Leveraging low-cost Virtual Private Servers (VPS) and free/low-cost services.
*   **Data Persistence:** Ensuring database data is preserved using Docker Volumes.
*   **Scalability Path:** Designing with future scaling in mind, allowing for upgrades as the project grows.

## Architectural Components

### 1. Client-Side Application (Angular/React)

This component involves serving your static HTML, CSS, and JavaScript files.

*   **CloudFlare:**
    *   **Type:** CDN, DNS, Security
    *   **Cost:** **Free Tier**
    *   **Details:** CloudFlare's free tier is more than sufficient for your MVP. It provides global CDN caching, basic DDoS protection, and DNS management. This will significantly speed up the initial loading of your client application for users worldwide.

*   **Static Hosting Provider Options:**

    *   **Option A: Netlify / Vercel**
        *   **Type:** Specialized Static Site Hosting
        *   **Cost:** **Free (Hobby/Starter Tiers)**
        *   **Details:** These providers are excellent for hosting static frontends. They offer continuous deployment from Git repositories, automatic SSL, and generous free tiers that typically include:
            *   **Netlify:** 100GB/month bandwidth, 300 build minutes/month.
            *   **Vercel:** 100GB/month bandwidth, 100 build minutes/day.
        *   **Pros:** Extremely easy to set up and manage, built-in CI/CD, highly optimized for static content.
        *   **Cons:** Less control over the underlying server if you ever need custom server-side logic for your static files (unlikely for a pure Angular/React app).

    *   **Option B: DigitalOcean Droplet (Smallest)**
        *   **Type:** Virtual Private Server (VPS)
        *   **Cost:** ~$6 - $8 USD/month (e.g., 1GB RAM, 1 vCPU, 25GB SSD, 1TB Transfer)
        *   **Details:** You would deploy a small Droplet (e.g., in a central location like New York or Amsterdam for good global peering) and configure a web server like Nginx or Apache to serve your static files.
        *   **Pros:** Full control over the server environment.
        *   **Cons:** Requires manual setup and maintenance of the web server. More expensive than free static hosting options.
        *   **Recommendation:** Start with Netlify or Vercel for the client-side. They are free and purpose-built for this.

### 2. Web API (.NET Core Web API)

This component involves running your .NET Core Web API in a Docker container. You need three instances, one in each city (Durban, JHB, Cape Town).

*   **Provider Options (VPS):**

    *   **Option A: DigitalOcean Droplets**
        *   **Type:** Virtual Private Server (VPS)
        *   **Cost:** ~$6 - $8 USD/month per Droplet (e.g., 1GB RAM, 1 vCPU, 25GB SSD, 1TB Transfer)
        *   **Total for 3 instances:** ~$18 - $24 USD/month
        *   **Details:** DigitalOcean has a Cape Town, South Africa region. You would deploy three separate Droplets, one in Cape Town, and two in other regions (e.g., London, Frankfurt, or New York) that offer good connectivity to Durban and JHB, or if DigitalOcean expands its SA presence. Each Droplet would run your Dockerized Web API.
        *   **Pros:** Simple pricing, easy to spin up, good performance for the price.
        *   **Cons:** Limited SA regions (currently only Cape Town).

    *   **Option B: Hetzner Cloud**
        *   **Type:** Virtual Private Server (VPS)
        *   **Cost:** ~$4 - $6 USD/month per instance (e.g., 1 vCPU, 2GB RAM, 20GB SSD, 20TB Transfer)
        *   **Total for 3 instances:** ~$12 - $18 USD/month
        *   **Details:** Hetzner has a strong presence in South Africa (Johannesburg). You could potentially deploy all three instances within SA (JHB and Cape Town, and perhaps another region if they expand or use a different provider for Durban). Their pricing is often very competitive for the resources provided.
        *   **Pros:** Excellent price-to-performance ratio, strong SA presence.
        *   **Cons:** Interface might be slightly less beginner-friendly than DigitalOcean for some.

    *   **Option C: OVHcloud**
        *   **Type:** Virtual Private Server (VPS)
        *   **Cost:** ~$5 - $7 USD/month per instance (e.g., 1 vCPU, 2GB RAM, 20GB SSD, 100Mbps Unmetered)
        *   **Total for 3 instances:** ~$15 - $21 USD/month
        *   **Details:** OVHcloud also has a South Africa (Johannesburg) region. Similar to Hetzner, you could aim for SA-based deployments.
        *   **Pros:** Competitive pricing, good network.
        *   **Cons:** Can be more complex to navigate for new users.

    *   **Recommendation:** Hetzner Cloud or DigitalOcean are strong contenders. Hetzner might offer better value in SA.

### 3. Database/Datastore (PostgreSQL)

This component involves a single, centralized PostgreSQL database instance.

*   **Provider Options (VPS):**

    *   **Option A: DigitalOcean Droplet (Slightly Larger)**
        *   **Type:** Virtual Private Server (VPS)
        *   **Cost:** ~$12 - $15 USD/month (e.g., 2GB RAM, 1 vCPU, 50GB SSD, 2TB Transfer)
        *   **Details:** Deploy a single Droplet in DigitalOcean's Cape Town, SA region. This provides more RAM and disk space for the database. You would run PostgreSQL in a Docker container with a mounted Docker Volume for persistence.
        *   **Pros:** Simple to manage, good performance.
        *   **Cons:** Only one SA region.

    *   **Option B: Hetzner Cloud (Slightly Larger)**
        *   **Type:** Virtual Private Server (VPS)
        *   **Cost:** ~$6 - $8 USD/month (e.g., 2 vCPU, 4GB RAM, 40GB SSD, 20TB Transfer)
        *   **Details:** Deploy a single instance in Hetzner's Johannesburg, SA region. Hetzner often provides more resources for the price, which is beneficial for a database.
        *   **Pros:** Excellent price-to-performance, strong SA presence.
        *   **Cons:** None significant for this use case.

    *   **Option C: OVHcloud (Slightly Larger)**
        *   **Type:** Virtual Private Server (VPS)
        *   **Cost:** ~$10 - $12 USD/month (e.g., 2 vCPU, 4GB RAM, 40GB SSD, Unmetered)
        *   **Details:** Deploy a single instance in OVHcloud's Johannesburg, SA region.
        *   **Pros:** Competitive pricing.
        *   **Cons:** None significant for this use case.

    *   **Recommendation:** Hetzner Cloud often provides the best value for money for VPS instances in South Africa, making it a strong candidate for your central database.

## Total Estimated Monthly Cost (Using Cost-Effective Options)

*   **Client-Side Application:**
    *   CloudFlare: $0
    *   Netlify/Vercel: $0
*   **Web API (3 instances, e.g., Hetzner Cloud):** ~$12 - $18
*   **Database (1 instance, e.g., Hetzner Cloud):** ~$6 - $8
*   **Domain Name:** ~$1/month (amortized from annual cost)

**Total Estimated Monthly Cost: ~$19 - $27 USD**

This leaves you well within your $300/month budget, providing significant headroom for potential increases in resource usage, additional services, or future scaling.

## Architectural Diagram

The following diagram illustrates the proposed architecture:

```mermaid
graph TD
    subgraph Users
        U_CP[Cape Town User]
        U_JHB[Johannesburg User]
        U_DBN[Durban User]
    end

    subgraph Internet
        CF[CloudFlare CDN]
    end

    subgraph South Africa (SA)
        subgraph Client-Side Hosting
            SCH[Static Client Host (e.g., DO Droplet)]
            SCH_Files(Static WebApp Files)
            SCH --> SCH_Files
        end

        subgraph Web API - Durban
            WAPI_DBN_VPS[Durban VPS]
            WAPI_DBN_Docker(Docker Container: .NET Web API)
            WAPI_DBN_VPS --> WAPI_DBN_Docker
        end

        subgraph Web API - Johannesburg
            WAPI_JHB_VPS[Johannesburg VPS]
            WAPI_JHB_Docker(Docker Container: .NET Web API)
            WAPI_JHB_VPS --> WAPI_JHB_Docker
        end

        subgraph Web API - Cape Town
            WAPI_CPT_VPS[Cape Town VPS]
            WAPI_CPT_Docker(Docker Container: .NET Web API)
            WAPI_CPT_VPS --> WAPI_CPT_Docker
        end

        subgraph Central Database - Johannesburg
            DB_JHB_VPS[Johannesburg DB VPS]
            DB_JHB_Docker(Docker Container: PostgreSQL)
            DB_JHB_Volume(Docker Volume: Persistent Data)
            DB_JHB_VPS --> DB_JHB_Docker
            DB_JHB_Docker --- DB_JHB_Volume
        end
    end

    U_CP -- Request --> CF
    U_JHB -- Request --> CF
    U_DBN -- Request --> CF

    CF -- Serve Static Files --> SCH
    CF -- API Request --> WAPI_DBN_VPS
    CF -- API Request --> WAPI_JHB_VPS
    CF -- API Request --> WAPI_CPT_VPS

    WAPI_DBN_Docker -- API Calls --> DB_JHB_Docker
    WAPI_JHB_Docker -- API Calls --> DB_JHB_Docker
    WAPI_CPT_Docker -- API Calls --> DB_JHB_Docker

    style U_CP fill:#f9f,stroke:#333,stroke-width:2px
    style U_JHB fill:#f9f,stroke:#333,stroke-width:2px
    style U_DBN fill:#f9f,stroke:#333,stroke-width:2px
    style CF fill:#bbf,stroke:#333,stroke-width:2px
    style SCH fill:#bbf,stroke:#333,stroke-width:2px
    style WAPI_DBN_VPS fill:#bbf,stroke:#333,stroke-width:2px
    style WAPI_JHB_VPS fill:#bbf,stroke:#333,stroke-width:2px
    style WAPI_CPT_VPS fill:#bbf,stroke:#333,stroke-width:2px
    style DB_JHB_VPS fill:#bbf,stroke:#333,stroke-width:2px
    style WAPI_DBN_Docker fill:#ccf,stroke:#333,stroke-width:2px
    style WAPI_JHB_Docker fill:#ccf,stroke:#333,stroke-width:2px
    style WAPI_CPT_Docker fill:#ccf,stroke:#333,stroke-width:2px
    style DB_JHB_Docker fill:#ccf,stroke:#333,stroke-width:2px
    style DB_JHB_Volume fill:#cfc,stroke:#333,stroke-width:2px
    style SCH_Files fill:#cfc,stroke:#333,stroke-width:2px
```