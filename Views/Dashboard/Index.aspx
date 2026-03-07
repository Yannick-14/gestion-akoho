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

    <div class="container">
        <h1 class="mb-4">Dashboard</h1>

        <% if (Model != null && Model.Any()) { %>
            <div class="row g-3">
                <% foreach (var lot in Model) { %>
                    <div class="col-12 col-md-6 col-xl-4">
                        <div class="card h-100 shadow-sm border-0">
                            <div class="card-body">
                                <div class="d-flex justify-content-between align-items-center mb-3">
                                    <h2 class="h5 mb-0"><%: lot.Lot.NomLot %></h2>
                                    <div>
                                        <span class="badge bg-secondary"><%: lot.Lot.Race != null ? lot.Lot.Race.Nom : "Inconnue" %> (ID: <%: lot.Lot.RaceId %>)</span>
                                        <span class="badge bg-primary">Lot</span>
                                    </div>
                                </div>

                                <div class="vstack gap-2 small">
                                    <div class="d-flex justify-content-between">
                                        <span class="text-muted">Nombre de contenu</span>
                                        <strong><%: lot.Lot.NombreInitial %></strong>
                                    </div>
                                    <div class="d-flex justify-content-between">
                                        <span class="text-muted">Monnaie investie</span>
                                        <strong><%: lot.Lot.TotalInvesti.ToString("N2") %> Ar</strong>
                                    </div>
                                    <div class="d-flex justify-content-between">
                                        <span class="text-muted">Reste nombre actuel</span>
                                        <strong><%: lot.ResteNombreActuel %></strong>
                                    </div>
                                    <div class="d-flex justify-content-between">
                                        <span class="text-muted">Mort</span>
                                        <strong class="text-danger"><%: lot.Mort %></strong>
                                    </div>
                                    <div class="d-flex justify-content-between">
                                        <span class="text-muted">Statu</span>
                                        <%
                                            if (lot.Lot.Statu == 0) {%>
                                                <strong class="text-success">Akoho</strong>
                                            <%} else { %>
                                                <strong class="text-success">Atody</strong>
                                            <%}
                                        %>
                                    </div>
                                    <div class="d-flex justify-content-between pt-1 border-top">
                                        <span class="text-muted">Benefice</span>
                                        <strong class="<%: lot.Benefice >= 0 ? "text-success" : "text-danger" %>">
                                            <%: lot.Benefice.ToString("N2") %> Ar
                                        </strong>
                                    </div>
                                    <a href="/DashBoard/MakaAtody?lotId=<%: lot.Lot.Id %>&raceId=<%: lot.Lot.RaceId %>" class="btn btn-primary">Maka Atody</a>
                                </div>
                            </div>
                        </div>
                    </div>
                <% } %>
            </div>
        <% } else { %>
            <div class="alert alert-info">Aucune donnee disponible pour le dashboard.</div>
        <% } %>
    </div>
</body>
</html>
