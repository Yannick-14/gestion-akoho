<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Dashboard.LotOeufsPage" %>
<%@ Import Namespace="System.Linq" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Lots d'Oeufs - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <style>
        :root {
            --egg-bg: #fffbf2;
            --egg-accent: #f59e0b; /* Amber */
            --egg-accent-soft: #fef3c7;
            --text-main: #1e293b;
            --card-border: #fde68a;
        }

        body {
            background-color: var(--egg-bg);
            background-image: radial-gradient(at 100% 0%, rgba(245, 158, 11, 0.05) 0px, transparent 50%),
                              radial-gradient(at 0% 100%, rgba(245, 158, 11, 0.03) 0px, transparent 50%);
            color: var(--text-main);
            font-family: 'Inter', -apple-system, sans-serif;
            min-height: 100vh;
        }

        .page-header {
            border-bottom: 2px solid var(--egg-accent-soft);
            padding-bottom: 1.5rem;
            margin-bottom: 2.5rem;
        }

        .egg-card {
            background: #ffffff;
            border: 1px solid var(--card-border);
            border-radius: 24px;
            box-shadow: 0 10px 15px -3px rgba(245, 158, 11, 0.05);
            transition: all 0.3s ease;
            overflow: hidden;
            position: relative;
        }

        .egg-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 20px 25px -5px rgba(245, 158, 11, 0.1);
            border-color: var(--egg-accent);
        }

        .egg-card-header {
            background: var(--egg-accent-soft);
            padding: 1rem 1.5rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .egg-id-badge {
            background: white;
            color: var(--egg-accent);
            font-weight: 800;
            padding: 0.2rem 0.6rem;
            border-radius: 8px;
            font-size: 0.75rem;
        }

        .egg-card-body {
            padding: 1.5rem;
        }

        .egg-quantity-box {
            background: linear-gradient(135deg, white 0%, var(--egg-bg) 100%);
            border: 2px dashed var(--card-border);
            border-radius: 16px;
            padding: 1.25rem;
            text-align: center;
            margin-bottom: 1.5rem;
        }

        .egg-quantity-value {
            display: block;
            font-size: 2rem;
            font-weight: 800;
            color: var(--egg-accent);
            line-height: 1;
        }

        .egg-quantity-label {
            font-size: 0.75rem;
            text-transform: uppercase;
            font-weight: 600;
            color: #92400e;
            letter-spacing: 0.05em;
        }

        .info-grid {
            display: grid;
            gap: 0.75rem;
            margin-bottom: 1.5rem;
        }

        .info-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding-bottom: 0.5rem;
            border-bottom: 1px solid #fff7ed;
        }

        .info-label {
            font-size: 0.8rem;
            color: #64748b;
        }

        .info-value {
            font-weight: 600;
            color: #1e293b;
        }

        .btn-eclore-premium {
            background: var(--egg-accent);
            color: white;
            border: none;
            padding: 0.8rem;
            border-radius: 12px;
            font-weight: 700;
            width: 100%;
            transition: all 0.2s;
        }

        .btn-eclore-premium:hover {
            background: #d97706;
            transform: scale(1.02);
            color: white;
        }

        .pulse-premium {
            animation: pulse-amber 2s infinite;
        }

        @keyframes pulse-amber {
            0% { box-shadow: 0 0 0 0 rgba(245, 158, 11, 0.4); }
            70% { box-shadow: 0 0 0 10px rgba(245, 158, 11, 0); }
            100% { box-shadow: 0 0 0 0 rgba(245, 158, 11, 0); }
        }
    </style>
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-5">
        <div class="page-header d-flex justify-content-between align-items-end">
            <div>
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-2">
                        <li class="breadcrumb-item"><a href="/Dashboard/Index" class="text-decoration-none text-muted">Dashboard</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Lots d'Oeufs</li>
                    </ol>
                </nav>
                <h1 class="h2 fw-bold mb-0">Gestion des Lots d'Oeufs</h1>
                <p class="text-muted mb-0">Visualisez et faites éclore vos récoltes d'Oeufs actives.</p>
            </div>
            <div class="text-end">
                <span class="badge bg-warning text-dark px-3 py-2 rounded-pill fw-bold">
                    <%: Model != null ? Model.LotOeufsActive.Count() : 0 %> Lots Actifs
                </span>
            </div>
        </div>

        <div class="row g-4">
            <% if (Model != null && Model.LotOeufsActive.Any()) { %>
                <% foreach (var lotOeuf in Model.LotOeufsActive) { 
                        bool estPret = lotOeuf.DateEclosion.HasValue && AkohoAspx.Utils.Time.GetDateActuelle() >= lotOeuf.DateEclosion.Value;
                %>
                    <div class="col-12 col-md-6 col-lg-4">
                        <div class="egg-card h-100">
                            <div class="egg-card-header">
                                <h3 class="h6 mb-0 fw-bold text-amber-900">Lot d'Oeufs</h3>
                                <span class="egg-id-badge">#<%: lotOeuf.Id %></span>
                            </div>
                            <div class="egg-card-body">
                                <div class="egg-quantity-box">
                                    <span class="egg-quantity-value"><%: lotOeuf.NbOeufs %></span>
                                    <span class="egg-quantity-label">Oeufs récoltés</span>
                                </div>
                                
                                <div class="info-grid">
                                    <div class="info-item">
                                        <span class="info-label">Race</span>
                                        <span class="info-value text-primary"><%: lotOeuf.Race != null ? lotOeuf.Race.Nom : "Inconnue" %></span>
                                    </div>
                                    <div class="info-item">
                                        <span class="info-label">Récolté le</span>
                                        <span class="info-value"><%: lotOeuf.Creation.ToString("dd MMM yyyy") %></span>
                                    </div>
                                    <div class="info-item" style="border-bottom: none;">
                                        <span class="info-label">Éclosion prévue</span>
                                        <span class="info-value"><%: lotOeuf.DateEclosion.HasValue ? lotOeuf.DateEclosion.Value.ToString("dd MMM yyyy") : "-" %></span>
                                    </div>
                                </div>

                                <a href="/Dashboard/EclosOeuf?lotOeufId=<%: lotOeuf.Id %>" 
                                   class="btn-eclore-premium text-decoration-none d-flex align-items-center justify-content-center gap-2 <%= estPret ? "pulse-premium" : "" %>">
                                   <% if (estPret) { %>
                                        ✨ Faire éclore maintenant
                                   <% } else { %>
                                        Voir les détails &rarr;
                                   <% } %>
                                </a>
                            </div>
                        </div>
                    </div>
                <% } %>
            <% } else { %>
                <div class="col-12">
                    <div class="text-center py-5 bg-white rounded-4 border">
                        <div class="display-1 text-muted mb-4 opacity-25">🥚</div>
                        <h2 class="h4 text-muted">Aucun lot d'Oeufs actif</h2>
                        <p class="text-muted">Les récoltes effectuées depuis les lots de poules s'afficheront ici.</p>
                        <a href="/Dashboard/Index" class="btn btn-outline-primary mt-3">Retour au Dashboard</a>
                    </div>
                </div>
            <% } %>
        </div>
    </div>
</body>
</html>
