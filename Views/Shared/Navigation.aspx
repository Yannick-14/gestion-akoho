<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Shared.NavigationPage" %>
<head>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700&display=swap" rel="stylesheet">
    <style>
        .navbar-premium {
            background: rgba(255, 255, 255, 0.8) !important;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
            border-bottom: 1px solid rgba(0, 0, 0, 0.05);
            padding: 0.75rem 0;
            position: sticky;
            top: 0;
            z-index: 1000;
        }

        .navbar-brand-premium {
            font-weight: 800;
            letter-spacing: -0.02em;
            color: #0f172a !important;
            font-size: 1.25rem;
        }

        .nav-link-premium {
            font-weight: 600;
            color: #475569 !important;
            padding: 0.5rem 1rem !important;
            border-radius: 8px;
            transition: all 0.2s;
            position: relative;
        }

        .nav-link-premium:hover {
            color: #0f172a !important;
            background: rgba(0, 0, 0, 0.03);
        }

        .nav-link-premium.active {
            color: #2563eb !important;
        }

        .nav-link-premium.active::after {
            content: "";
            position: absolute;
            bottom: 0;
            left: 1rem;
            right: 1rem;
            height: 3px;
            background: #2563eb;
            border-radius: 10px;
        }

        .date-badge {
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 10px;
            padding: 0.4rem 0.8rem;
            font-size: 0.8rem;
            font-weight: 600;
            color: #64748b;
        }

        .btn-reset-premium {
            border-radius: 8px;
            font-weight: 700;
            font-size: 0.75rem;
            padding: 0.4rem 0.8rem;
            text-transform: uppercase;
        }
    </style>
</head>

<nav class="navbar navbar-expand-lg navbar-light navbar-premium mb-4">
    <div class="container">
        <a class="navbar-brand navbar-brand-premium d-flex align-items-center gap-2" href="/">
            <span class="bg-primary text-white rounded-2 px-2 py-1" style="font-size: 0.9rem;">AK</span>
            Akoho ASPX
        </a>
        
        <button class="navbar-toggler border-0" type="button" data-bs-toggle="collapse" data-bs-target="#navbarMain">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarMain">
            <% 
                var controllerValue = ViewContext.RouteData.Values["controller"];
                var actionValue = ViewContext.RouteData.Values["action"];
                string currentController = controllerValue != null ? controllerValue.ToString() : string.Empty;
                string currentAction = actionValue != null ? actionValue.ToString() : string.Empty;
            %>
            <ul class="navbar-nav me-auto mb-2 mb-lg-0 ms-lg-4 gap-1">
                <li class="nav-item">
                    <a class="nav-link nav-link-premium <%= currentController == "Home" ? "active" : "" %>" 
                       href="/">Accueil</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link nav-link-premium <%= currentController == "Dashboard" && currentAction == "Index" ? "active" : "" %>" 
                       href="/Dashboard">Tableau de Bord</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link nav-link-premium <%= currentController == "Dashboard" && currentAction == "LotOeufs" ? "active" : "" %>" 
                       href="/Dashboard/LotOeufs">Lots d'Œufs</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link nav-link-premium <%= currentController == "Lot" ? "active" : "" %>" 
                       href="/Lot">Gestion des Lots</a>
                </li>
            </ul>
            
            <div class="d-flex align-items-center gap-3">
                <div class="date-badge">
                    <span class="opacity-75 me-1">📅</span>
                    <%: AkohoAspx.Utils.Time.GetDateActuelle().ToString("dd MMM yyyy") %>
                </div>
                <form action="/Dashboard/ResetDate" method="post" class="m-0">
                    <button type="submit" class="btn btn-outline-danger btn-reset-premium">Reset</button>
                </form>
            </div>
        </div>
    </div>
</nav>
