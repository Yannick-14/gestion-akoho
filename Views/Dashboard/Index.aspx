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
                                        <div><strong>Nombre:</strong> <%: lot.NombreInitial %></div>
                                        <div><strong>Creation:</strong> <%: lot.Creation.ToString("yyyy-MM-dd HH:mm") %></div>
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
