<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Dashboard.DashboardIndexPage" %>
<%@ Import Namespace="System.Linq" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Dashboard - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-4">
        <h1 class="mb-4">Dashboard</h1>

        <% var lotOeufsActifs = Model != null ? Model.LotOeufsActive : Enumerable.Empty<AkohoAspx.Models.LotOeuf>(); %>
        <% var lots = Model != null ? Model.Lots : Enumerable.Empty<AkohoAspx.Models.Lot>(); %>
        <section class="mb-3">
            <div class="col-12 col-md-4">
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
                            <div class="card h-100 shadow-sm border-0">
                                <div class="card-body">
                                    <h3 class="h6 mb-3">LotOeuf #<%: lotOeuf.Id %></h3>
                                    <div class="small vstack gap-1">
                                        <div><strong>Lot parent:</strong> <%: lotOeuf.LotParentId %></div>
                                        <div><strong>Race:</strong> <%: lotOeuf.Race != null ? lotOeuf.Race.Nom : lotOeuf.RaceId.ToString() %></div>
                                        <div><strong>Nombre oeufs:</strong> <%: lotOeuf.NbOeufs %></div>
                                        <div><strong>Pourcentage:</strong> <%: lotOeuf.Pourcentage %></div>
                                        <div><strong>Validation:</strong> <%: lotOeuf.Validation ? "true" : "false" %></div>
                                    </div>
                                    <hr class="my-1 border-secondary" />
                                    <a href="/Dashboard/MakaAtody?lotId=<%: lotOeuf.Id %>" class="btn btn-primary">Eclore &rarr;</a>
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
                <div class="row g-3">
                    <% foreach (var lot in lots) { %>
                        <div class="col-12 col-md-6 col-xl-4">
                            <div class="card h-100 shadow-sm border-0">
                                <div class="card-body">
                                    <h3 class="h6 mb-3"><%: lot.NomLot %></h3>
                                    <div class="small vstack gap-1">
                                        <div><strong>ID:</strong> <%: lot.Id %></div>
                                        <div><strong>Race:</strong> <%: lot.Race != null ? lot.Race.Nom : lot.RaceId.ToString() %></div>
                                        <div><strong>Nombre Initial:</strong> <%: lot.NombreInitial %></div>
                                        <div><strong>Reste Actuel:</strong> <%: Model != null && Model.ResteActuelLots.ContainsKey(lot.Id) ? Model.ResteActuelLots[lot.Id] : lot.NombreInitial %></div>
                                        <div><strong>PoidsInitiale:</strong> <%: lot.PoidsInitiale %></div>
                                        <div><strong>PrixAchat:</strong> <%: lot.PrixAchat %></div>
                                        <div><strong>Creation:</strong> <%: lot.Creation.ToString("yyyy-MM-dd HH:mm") %></div>
                                        <div><strong>Morts: </strong><%: Model.TotalMortLots[lot.Id] %></div>
                                        <hr class="my-1 border-secondary" />
                                        <div><strong>Semaine ecouler:</strong> <%: Model != null && Model.SemaineEcouler.ContainsKey(lot.Id) ? Model.SemaineEcouler[lot.Id] + " " : "N/A" %></div>
                                        <div><strong>Poids Actuel (U.):</strong> <%: Model != null && Model.PoidsFinalUnitaireLots.ContainsKey(lot.Id) ? Model.PoidsFinalUnitaireLots[lot.Id] + " g" : "N/A" %></div>
                                        <div><strong>Total Nourriture :</strong> <span class="text-danger"><%: Model != null && Model.PrixTotalNourritureLots.ContainsKey(lot.Id) ? Model.PrixTotalNourritureLots[lot.Id].ToString("N2") + " Ar" : "0.00 Ar" %></span></div>
                                        <div><strong>Valeur Vente Estimée :</strong> <span class="text-success"><%: Model != null && Model.PrixVenteLots.ContainsKey(lot.Id) ? Model.PrixVenteLots[lot.Id].ToString("N2") + " Ar" : "0.00 Ar" %></span></div>
                                        <div><strong>Prix de vente race / g:</strong> <%: Model != null && Model.PrixVenteRaceUnitaireLots.ContainsKey(lot.Id) ? Model.PrixVenteRaceUnitaireLots[lot.Id].ToString("N2") + " Ar" : "N/A" %></div>
                                        <div><strong>Prix de vente de lot:</strong> <span class="text-success"><%: Model != null && Model.PrixVenteLots.ContainsKey(lot.Id) ? Model.PrixVenteLots[lot.Id].ToString("N2") + " Ar" : "0.00 Ar" %></span></div>
                                        <div>
                                            <strong>Benefice Actuel:</strong> 
                                            <% 
                                                decimal depenseNourriture = Model != null && Model.PrixTotalNourritureLots.ContainsKey(lot.Id) ? Model.PrixTotalNourritureLots[lot.Id] : 0;
                                                decimal valeurVente = Model != null && Model.PrixVenteLots.ContainsKey(lot.Id) ? Model.PrixVenteLots[lot.Id] : 0;
                                                decimal benefice = valeurVente - depenseNourriture - lot.PrixAchat;
                                            %>
                                            <span class="<%: benefice >= 0 ? "text-success fw-bold" : "text-danger fw-bold" %>"><%: benefice.ToString("N2") %> Ar</span>
                                        </div>
                                        <hr class="my-1 border-secondary" />
                                        <a href="/Dashboard/MakaAtody?lotId=<%: lot.Id %>&raceId=<%: lot.RaceId %>" class="btn btn-primary">Maka Atody &rarr;</a>
                                        <a href="/Dashboard/SignalerMaty?lotId=<%: lot.Id %>" class="btn btn-success">Signaler maty &rarr;</a>
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
</body>
</html>
