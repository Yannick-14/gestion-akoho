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
            --dashboard-bg: #f4f7fb;
            --card-bg: #ffffff;
            --card-border: #dbe4ef;
            --text-main: #122033;
            --text-muted: #5b6f88;
            --kpi-bg: #f8fbff;
            --kpi-border: #e4ecf6;
        }

        body {
            background: radial-gradient(circle at top right, #edf4ff 0%, var(--dashboard-bg) 48%, #f7fbff 100%);
            color: var(--text-main);
        }

        .dashboard-title {
            font-weight: 700;
            letter-spacing: 0.25px;
        }

        .panel-soft {
            background: rgba(255, 255, 255, 0.9);
            border: 1px solid var(--card-border);
            border-radius: 18px;
            box-shadow: 0 10px 24px rgba(17, 40, 70, 0.08);
        }

        .egg-card {
            border-radius: 14px;
            border: 1px solid #dce8f7;
            background: linear-gradient(165deg, #ffffff 0%, #f7fbff 100%);
        }

        .lot-card {
            background: var(--card-bg);
            border: 1px solid var(--card-border);
            border-radius: 18px;
            box-shadow: 0 12px 30px rgba(11, 31, 58, 0.09);
            transition: transform 0.18s ease, box-shadow 0.18s ease, border-color 0.18s ease;
        }

        .lot-card:hover {
            transform: translateY(-3px);
            border-color: #bad0e8;
            box-shadow: 0 16px 34px rgba(11, 31, 58, 0.14);
        }

        .lot-card-click {
            cursor: pointer;
        }

        .lot-card-title {
            font-size: 1.04rem;
            font-weight: 700;
            color: var(--text-main);
        }

        .kpi-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 0.65rem;
        }

        .kpi-pill {
            background: var(--kpi-bg);
            border: 1px solid var(--kpi-border);
            border-radius: 12px;
            padding: 0.6rem 0.7rem;
            min-height: 72px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }

        .kpi-label {
            font-size: 0.74rem;
            color: var(--text-muted);
            text-transform: uppercase;
            letter-spacing: 0.3px;
            font-weight: 600;
        }

        .kpi-value {
            font-size: 0.95rem;
            font-weight: 700;
            color: var(--text-main);
            line-height: 1.2;
        }

        .lot-actions {
            border-top: 1px solid #e8eef7;
            margin-top: auto;
            padding-top: 0.95rem;
        }

        .btn-action-blue {
            background: #1f6feb;
            border-color: #1f6feb;
            color: #fff;
        }

        .btn-action-blue:hover {
            background: #1b5fca;
            border-color: #1b5fca;
            color: #fff;
        }

        .btn-action-red {
            background: #d32f2f;
            border-color: #d32f2f;
            color: #fff;
        }

        .btn-action-red:hover {
            background: #b82727;
            border-color: #b82727;
            color: #fff;
        }

        .modal-kpi {
            border: 1px solid #e3ebf5;
            background: #f9fcff;
            border-radius: 11px;
            padding: 0.7rem 0.75rem;
            height: 100%;
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

        <section class="mb-5">
            <h2 class="h4 mb-3">LotOeufs actifs</h2>
            <% if (lotOeufsActifs.Any()) { %>
                <div class="row g-3">
                    <% foreach (var lotOeuf in lotOeufsActifs) { %>
                        <div class="col-12 col-md-6 col-xl-4">
                            <div class="card h-100 egg-card shadow-sm">
                                <div class="card-body">
                                    <h3 class="h6 mb-3">LotOeuf #<%: lotOeuf.Id %></h3>
                                    <div class="small vstack gap-1">
                                        <div><strong>Creation:</strong> <%: lotOeuf.Creation.ToString("yyyy-MM-dd HH:mm") %></div>
                                        <div><strong>Lot parent:</strong> <%: lotOeuf.LotParentId %></div>
                                        <div><strong>Race:</strong> <%: lotOeuf.Race != null ? lotOeuf.Race.Nom : lotOeuf.RaceId.ToString() %></div>
                                        <div><strong>Nombre oeufs:</strong> <%: lotOeuf.NbOeufs %></div>
                                        <div><strong>Date eclosion:</strong> <%: lotOeuf.DateEclosion.HasValue ? lotOeuf.DateEclosion.Value.ToString("yyyy-MM-dd") : "" %></div>
                                    </div>
                                    <a href="/Dashboard/EclosOeuf?lotOeufId=<%: lotOeuf.Id %>" class="btn btn-primary">Eclore &rarr;</a>
                                    <% if (lotOeuf.DateEclosion.HasValue && AkohoAspx.Utils.Time.GetDateActuelle() >= lotOeuf.DateEclosion.Value) { %>
                                        <hr class="my-1 border-secondary" />
                                        <a href="/Dashboard/EclosOeuf?lotOeufId=<%: lotOeuf.Id %>" class="btn btn-primary">Eclore &rarr;</a>
                                    <% } %>
                                </div>
                            </div>
                        </div>
                    <% } %>
                </div>
            <% } else { %>
                <div class="alert alert-info">Aucun lotOeuf actif.</div>
            <% } %>
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
                                        <span class="badge text-bg-light border">Lot #<%: lot.Id %></span>
                                    </div>

                                    <div class="kpi-grid">
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Prix achat</span>
                                            <span class="kpi-value"><%: lot.PrixAchat.ToString("N2") %> Ar</span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Nourriture depense</span>
                                            <span class="kpi-value text-danger"><%: depenseNourriture.ToString("N2") %> Ar</span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Nombre de mort</span>
                                            <span class="kpi-value"><%: nombreMort %></span>
                                        </div>
                                        <div class="kpi-pill">
                                            <span class="kpi-label">Nombre actuel</span>
                                            <span class="kpi-value"><%: nombreActuel %></span>
                                        </div>
                                        <div class="kpi-pill" style="grid-column: span 2;">
                                            <span class="kpi-label">Prix du lot actuel si vente</span>
                                            <span class="kpi-value text-success"><%: prixVenteLot.ToString("N2") %> Ar</span>
                                        </div>
                                    </div>
                                </div>

                                <div class="lot-actions px-3 px-lg-4 pb-3 pb-lg-4 d-grid gap-2">
                                    <a href="/Dashboard/MakaAtody?lotId=<%: lot.Id %>&raceId=<%: lot.RaceId %>" class="btn btn-action-blue">Maka Atody &rarr;</a>
                                    <a href="/Dashboard/SignalerMaty?lotId=<%: lot.Id %>" class="btn btn-action-red">Signaler maty &rarr;</a>
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
                                        <a href="/Dashboard/MakaAtody?lotId=<%: lot.Id %>&raceId=<%: lot.RaceId %>" class="btn btn-action-blue">Maka Atody</a>
                                        <a href="/Dashboard/SignalerMaty?lotId=<%: lot.Id %>" class="btn btn-action-red">Signaler maty</a>
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
