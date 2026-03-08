<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Signaler une Perte - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700;800&display=swap" rel="stylesheet">
    <style>
        :root {
            --loss-bg: #fffafa;
            --loss-accent: #e11d48; /* Rose 600 */
            --loss-accent-soft: #ffe4e6;
            --glass-bg: rgba(255, 255, 255, 0.7);
            --text-main: #1e293b;
            --text-muted: #64748b;
        }

        body {
            background-color: var(--loss-bg);
            background-image: radial-gradient(at 100% 0%, rgba(225, 29, 72, 0.05) 0px, transparent 50%),
                              radial-gradient(at 0% 100%, rgba(225, 29, 72, 0.03) 0px, transparent 50%);
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
            background: linear-gradient(135deg, var(--loss-accent) 0%, #be123c 100%);
            padding: 2rem;
            border-radius: 28px 28px 0 0;
            color: white;
        }

        .info-card-soft {
            background: rgba(225, 29, 72, 0.03);
            border: 1px solid var(--loss-accent-soft);
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
            border-color: var(--loss-accent);
            box-shadow: 0 0 0 4px rgba(225, 29, 72, 0.1);
        }

        .btn-premium-rose {
            background: var(--loss-accent);
            color: white;
            border: none;
            padding: 1rem;
            border-radius: 14px;
            font-weight: 700;
            font-size: 1.1rem;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }

        .btn-premium-rose:hover {
            background: #be123c;
            transform: translateY(-2px);
            box-shadow: 0 10px 20px -5px rgba(190, 18, 60, 0.4);
            color: white;
        }
    </style>
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-5 mt-lg-4">
        <div class="row justify-content-center">
            <div class="col-12 col-md-8 col-lg-5">
                <div class="glass-panel overflow-hidden">
                    <div class="form-header d-flex justify-content-between align-items-center">
                        <div>
                            <h1 class="h3 fw-800 mb-0">Signaler une Perte</h1>
                            <p class="mb-0 opacity-75 small fw-600">Mettre à jour l'effectif du lot</p>
                        </div>
                        <a href="/Dashboard/Index" class="btn btn-sm btn-light text-danger fw-bold px-3 rounded-pill" style="color: #be123c !important;">&larr; Retour</a>
                    </div>
                    
                    <div class="p-4 p-lg-5">
                        <div class="info-card-soft mb-4">
                            <div class="d-flex align-items-center gap-3">
                                <div class="bg-white rounded-3 p-2 shadow-sm">📊</div>
                                <div>
                                    <div class="text-muted small fw-600">Lot d'origine</div>
                                    <div class="fw-bold">Lot #<%: ViewBag.LotId %></div>
                                </div>
                            </div>
                        </div>

                        <form action="/MouvementLot/Restriction" method="post">
                            <%: Html.AntiForgeryToken() %>
                            <input type="hidden" name="lotId" value="<%: ViewBag.LotId %>" />

                            <div class="mb-4">
                                <label for="NombreSortie" class="form-label fw-700 text-muted small text-uppercase">Nombre de pertes à déclarer</label>
                                <input type="number" class="form-control input-premium" id="NombreSortie" name="NombreSortie" min="1" placeholder="Ex: 5" required />
                                <div class="form-text mt-2 small">Ce nombre sera déduit de l'effectif actuel.</div>
                            </div>

                            <div class="d-grid pt-2">
                                <button type="submit" class="btn btn-premium-rose shadow-sm">
                                    Confirmer la Perte
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
