<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Dashboard.DashboardIndexPage" %>
<%@ Import Namespace="System.Linq" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Dashboard - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <style>
        :root {
            --dashboard-bg: #f0f4f8;
            --card-bg: #ffffff;
            --card-border: #e2e8f0;
            --text-main: #1e293b;
            --text-muted: #64748b;
            --kpi-bg: #f8fafc;
            --kpi-border: #f1f5f9;
            --accent-blue: #3b82f6;
            --accent-red: #ef4444;
            --accent-green: #10b981;
        }

        body {
            background-color: var(--dashboard-bg);
            background-image: radial-gradient(at 0% 0%, rgba(59, 130, 246, 0.05) 0px, transparent 50%),
                              radial-gradient(at 50% 0%, rgba(16, 185, 129, 0.05) 0px, transparent 50%);
            color: var(--text-main);
            font-family: 'Inter', -apple-system, sans-serif;
        }

        .dashboard-title {
            font-weight: 800;
            color: #0f172a;
            font-size: 2.25rem;
        }

        .panel-soft {
            background: rgba(255, 255, 255, 0.8);
            backdrop-filter: blur(12px);
            border: 1px solid var(--card-border);
            border-radius: 20px;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
        }

        .egg-card {
            border-radius: 14px;
            border: 1px solid #dce8f7;
            background: linear-gradient(165deg, #ffffff 0%, #f7fbff 100%);
        }

        .lot-card {
            background: var(--card-bg);
            border: 1px solid var(--card-border);
            border-radius: 24px;
            box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.04);
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }

        .lot-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.08);
            border-color: var(--accent-blue);
        }

        .lot-card-click {
            cursor: pointer;
        }

        .lot-card-title {
            font-size: 1.25rem;
            font-weight: 700;
            color: #0f172a;
        }

        .kpi-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 0.65rem;
        }

        .kpi-pill {
            background: var(--kpi-bg);
            border: 1px solid var(--kpi-border);
            border-radius: 16px;
            padding: 1rem;
            display: flex;
            flex-direction: column;
            gap: 0.25rem;
        }

        .kpi-label {
            font-size: 0.75rem;
            color: var(--text-muted);
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.025em;
        }

        .kpi-value {
            font-size: 1.1rem;
            font-weight: 700;
            color: #1e293b;
        }

        .lot-actions {
            border-top: 1px solid #e8eef7;
            margin-top: auto;
            padding-top: 0.95rem;
        }

        .btn-action-blue {
            background: var(--accent-blue);
            color: white;
            border-radius: 12px;
            font-weight: 600;
            padding: 0.6rem;
            border: none;
            transition: all 0.2s;
        }

        .btn-action-blue:hover { background: #2563eb; color: white; }

        .btn-action-red {
            background: #fee2e2;
            color: var(--accent-red);
            border-radius: 12px;
            font-weight: 600;
            padding: 0.6rem;
            border: none;
            transition: all 0.2s;
        }

        .btn-action-red:hover { background: #dc2626; color: white; }

        .modal-kpi {
            background: #f8fafc;
            border: 1px solid #f1f5f9;
            border-radius: 14px;
            padding: 1rem;
        }

        .modal-kpi span {
            display: block;
            font-size: 0.74rem;
            color: #607895;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            font-weight: 600;
        }

        .modal-kpi strong {
            font-size: 0.96rem;
            color: #0f243d;
        }
    </style>
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-4">
        <h1 class="mb-4 dashboard-title">Dashboard</h1>

        <% var lotOeufsActifs = Model != null ? Model.LotOeufsActive : Enumerable.Empty<AkohoAspx.Models.LotOeuf>(); %>
        <% var lots = Model != null ? Model.Lots : Enumerable.Empty<AkohoAspx.Models.Lot>(); %>

        <section class="mb-4">
            <div class="col-12 col-md-5 col-lg-4 panel-soft p-3">
                <form action="/Dashboard/ActualiserDate" method="post" class="d-flex align-items-end gap-2">
                    <div class="flex-grow-1">
                        <label for="DateActualiser" class="form-label">Personnaliser la date actuelle</label>
                        <input id="DateActualiser" name="DateActualiser" type="date" class="form-control" value="<%: AkohoAspx.Utils.Time.GetDateActuelle().ToString("yyyy-MM-dd") %>" />
                    </div>
                    <button type="submit" class="btn btn-primary">Actualiser</button>
                </form>
            </div>
        </section>



        <section>
            <h2 class="h4 mb-3">Tous les lots</h2>
            <% if (lots.Any()) { %>
                <div class="row g-4">
                    <% foreach (var lot in lots) { %>
                        <%
                            int nombreActuel = Model != null && Model.ResteActuelLots.ContainsKey(lot.Id) ? Model.ResteActuelLots[lot.Id] : lot.NombreInitial;
                            int nombreMort = Model != null && Model.TotalMortLots.ContainsKey(lot.Id) ? Model.TotalMortLots[lot.Id] : 0;
                            decimal depenseNourriture = Model != null && Model.PrixTotalNourritureLots.ContainsKey(lot.Id) ? Model.PrixTotalNourritureLots[lot.Id] : 0;
                            decimal prixVenteLot = Model != null && Model.PrixVenteLots.ContainsKey(lot.Id) ? Model.PrixVenteLots[lot.Id] : 0;
                            int semaineEcoulee = Model != null && Model.SemaineEcouler.ContainsKey(lot.Id) ? Model.SemaineEcouler[lot.Id] : 0;
                            int poidsActuelUnitaire = Model != null && Model.PoidsFinalUnitaireLots.ContainsKey(lot.Id) ? Model.PoidsFinalUnitaireLots[lot.Id] : 0;
                            decimal prixVenteRaceUnitaire = Model != null && Model.PrixVenteRaceUnitaireLots.ContainsKey(lot.Id) ? Model.PrixVenteRaceUnitaireLots[lot.Id] : 0;
                            decimal benefice = prixVenteLot - (depenseNourriture + lot.PrixAchat);
                        %>

                        <div class="col-12 col-md-6 col-xl-4 d-flex">
                            <div class="lot-card d-flex flex-column w-100" role="button" data-bs-toggle="modal" data-bs-target="#lotModal_<%: lot.Id %>" aria-label="Voir details lot <%: lot.NomLot %>">
                                <div class="lot-card-click p-3 p-lg-4">
                                    <div class="d-flex justify-content-between align-items-start gap-2 mb-3">
                                        <h3 class="lot-card-title mb-0"><%: lot.NomLot %></h3>
                                        <div class="d-flex flex-column align-items-end gap-1">
                                            <span class="badge text-bg-light border shadow-sm">Lot #<%: lot.Id %></span>
                                            <% 
                                                int maxWeek = Model != null && Model.MaxSemaineCroissanceLots.ContainsKey(lot.Id) ? Model.MaxSemaineCroissanceLots[lot.Id] : 0;
                                                if (semaineEcoulee >= maxWeek && maxWeek > 0) { 
                                            %>
                                                <span class="badge rounded-pill shadow-sm" style="background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; font-size: 0.75rem; border: 1px solid rgba(255,255,255,0.2);">
                                                    ✅ Prêt à vendre
                                                </span>
                                            <% } else { %>
                                                <span class="badge bg-white text-muted border rounded-pill shadow-sm" style="font-size: 0.7rem;">
                                                    Sem. <%: semaineEcoulee %> / <%: maxWeek %>
                                                </span>
                                            <% } %>
                                        </div>
                                    </div>

                                    <div class="kpi-grid">
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Croissance</span>
                                            <span class="kpi-value text-primary">Semaine <%: semaineEcoulee %></span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Prix d'achat</span>
                                            <span class="kpi-value"><%: lot.PrixAchat.ToString("N2") %> Ar</span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Dépense nourriture</span>
                                            <span class="kpi-value text-danger"><%: depenseNourriture.ToString("N2") %> Ar</span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Nombre de pertes</span>
                                            <span class="kpi-value"><%: nombreMort %></span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Nombre actuel</span>
                                            <span class="kpi-value"><%: nombreActuel %></span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Poids unitaire</span>
                                            <span class="kpi-value fw-bold"><%: poidsActuelUnitaire %> g</span>
                                        </div>
                                        <div class="kpi-pill" style="grid-column: span 2;">
                                            <span class="kpi-label">Prix du lot actuel si vente</span>
                                            <span class="kpi-value text-success"><%: prixVenteLot.ToString("N2") %> Ar</span>
                                        </div>
                                    </div>
                                </div>

                                <div class="lot-actions px-3 px-lg-4 pb-3 pb-lg-4 d-grid gap-2">
                                    <a href="/Dashboard/MakaAtody?lotId=<%: lot.Id %>&raceId=<%: lot.RaceId %>" class="btn btn-action-blue">Récolter les Oeufs &rarr;</a>
                                    <a href="/Dashboard/SignalerMaty?lotId=<%: lot.Id %>" class="btn btn-action-red">Signaler une perte &rarr;</a>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="lotModal_<%: lot.Id %>" tabindex="-1" aria-labelledby="lotModalLabel_<%: lot.Id %>" aria-hidden="true">
                            <div class="modal-dialog modal-lg modal-dialog-centered">
                                <div class="modal-content border-0 shadow">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="lotModalLabel_<%: lot.Id %>"><%: lot.NomLot %> - details</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row g-2">
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>ID</span><strong><%: lot.Id %></strong></div>
                                            </div>
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>Race</span><strong><%: lot.Race != null ? lot.Race.Nom : lot.RaceId.ToString() %></strong></div>
                                            </div>
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>Nombre initial</span><strong><%: lot.NombreInitial %></strong></div>
                                            </div>
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>Poids initial</span><strong><%: lot.PoidsInitiale %> g</strong></div>
                                            </div>
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>Semaine ecoulee</span><strong><%: semaineEcoulee %></strong></div>
                                            </div>
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>Poids actuel (U.)</span><strong><%: poidsActuelUnitaire > 0 ? poidsActuelUnitaire + " g" : "N/A" %></strong></div>
                                            </div>
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>Prix vente race / g</span><strong><%: prixVenteRaceUnitaire > 0 ? prixVenteRaceUnitaire.ToString("N2") + " Ar" : "N/A" %></strong></div>
                                            </div>
                                            <div class="col-6 col-md-4">
                                                <div class="modal-kpi"><span>Creation</span><strong><%: lot.Creation.ToString("yyyy-MM-dd HH:mm") %></strong></div>
                                            </div>
                                            <div class="col-12 col-md-4">
                                                <div class="modal-kpi">
                                                    <span>Benefice actuel</span>
                                                    <strong class="<%: benefice >= 0 ? "text-success" : "text-danger" %>"><%: benefice.ToString("N2") %> Ar</strong>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <a href="/Dashboard/MakaAtody?lotId=<%: lot.Id %>&raceId=<%: lot.RaceId %>" class="btn btn-action-blue">Récolter les Oeufs</a>
                                        <a href="/Dashboard/SignalerMaty?lotId=<%: lot.Id %>" class="btn btn-action-red">Signaler une perte</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    <% } %>
                </div>
            <% } else { %>
                <div class="alert alert-info">Aucun lot disponible.</div>
            <% } %>
        </section>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        document.querySelectorAll('.lot-actions a').forEach(function (link) {
            link.addEventListener('click', function (event) {
                event.stopPropagation();
            });
        });
    </script>
</body>
</html>
