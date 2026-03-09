<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Éclosion - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700;800&display=swap" rel="stylesheet">
    <style>
        :root {
            --hatch-bg: #f0f9ff;
            --hatch-accent: #0ea5e9;
            --hatch-blue-soft: #e0f2fe;
            --glass-bg: rgba(255, 255, 255, 0.75);
            --text-main: #0f172a;
        }

        body {
            background-color: var(--hatch-bg);
            background-image: radial-gradient(at 100% 0%, rgba(14, 165, 233, 0.08) 0px, transparent 50%),
                              radial-gradient(at 0% 100%, rgba(14, 165, 233, 0.04) 0px, transparent 50%);
            color: var(--text-main);
            font-family: 'Inter', sans-serif;
            min-height: 100vh;
        }

        .glass-panel {
            background: var(--glass-bg);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border: 1px solid rgba(255, 255, 255, 0.4);
            border-radius: 28px;
            box-shadow: 0 20px 40px -10px rgba(0, 0, 0, 0.05);
        }

        .form-header {
            background: linear-gradient(135deg, var(--hatch-accent) 0%, #0284c7 100%);
            padding: 2rem;
            border-radius: 28px 28px 0 0;
            color: white;
        }

        .info-card-soft {
            background: rgba(14, 165, 233, 0.04);
            border: 1px solid var(--hatch-blue-soft);
            border-radius: 18px;
            padding: 1.25rem;
        }

        .input-premium {
            border: 2px solid #f1f5f9;
            border-radius: 14px;
            padding: 0.8rem 1rem;
            font-weight: 600;
            transition: all 0.2s;
        }

        .input-premium:focus {
            border-color: var(--hatch-accent);
            box-shadow: 0 0 0 4px rgba(14, 165, 233, 0.1);
        }

        .btn-premium-hatch {
            background: var(--hatch-accent);
            color: white;
            border: none;
            padding: 1rem;
            border-radius: 14px;
            font-weight: 700;
            font-size: 1.16rem;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }

        .btn-premium-hatch:hover {
            background: #0284c7;
            transform: translateY(-2px);
            box-shadow: 0 10px 20px -5px rgba(14, 165, 233, 0.4);
            color: white;
        }
    </style>
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-5 mt-lg-4">
        <div class="row justify-content-center">
            <div class="col-12 col-md-8 col-lg-6">
                <div class="glass-panel overflow-hidden">
                    <div class="form-header d-flex justify-content-between align-items-center">
                        <div>
                            <h1 class="h3 fw-800 mb-0">Éclosion des Oeufs</h1>
                            <p class="mb-0 opacity-75 small fw-600">Transformer le lot d'Oeufs en poussins</p>
                        </div>
                        <a href="/Dashboard/LotOeufs" class="btn btn-sm btn-light fw-bold px-3 rounded-pill" style="color: var(--hatch-accent) !important;">&larr; Retour</a>
                    </div>
                    
                    <div class="p-4 p-lg-5">
                        <div class="info-card-soft mb-4">
                            <div class="d-flex align-items-center gap-3">
                                <div class="bg-white rounded-3 p-2 shadow-sm">🐣</div>
                                <div>
                                    <div class="text-muted small fw-600">Lot d'Oeufs cible</div>
                                    <div class="fw-bold">Lot #<%: ViewBag.LotOeufId %></div>
                                </div>
                            </div>
                        </div>

                        <form action="/Lot/CreateNewLot" method="post">
                            <%: Html.AntiForgeryToken() %>
                            <input type="hidden" name="lotOeufId" value="<%: ViewBag.LotOeufId %>" />

                            <div class="row g-4 mb-4">
                                <div class="col-12 col-md-5">
                                    <label for="pourcentage" class="form-label fw-700 text-muted small text-uppercase">Taux d'éclosion (%)</label>
                                    <input type="number" class="form-control input-premium" id="pourcentage" name="pourcentage" min="1" max="100" placeholder="Ex: 85" required />
                                </div>

                                <div class="col-12 col-md-7">
                                    <label for="nomLot" class="form-label fw-700 text-muted small text-uppercase">Nom du nouveau lot</label>
                                    <input id="nomLot" name="nomLot" class="form-control input-premium" placeholder="Ex: Lot Poussins Mars" required />
                                </div>
                            </div>

                            <div class="d-grid pt-2">
                                <button type="submit" class="btn btn-premium-hatch shadow-sm">
                                    Finaliser l'Éclosion & Créer le Lot
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
