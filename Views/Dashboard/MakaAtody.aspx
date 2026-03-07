<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Maka Atody - Elevage Akoho</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body class="bg-light">
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-8 col-lg-6">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                        <h1 class="h4 mb-0">Maka Atody</h1>
                        <a href="/DashBoard/Index" class="btn btn-sm btn-light text-primary fw-bold">&larr; Retour</a>
                    </div>
                    <div class="card-body p-4">
                        <div class="alert alert-info mb-4">
                            <strong>Details du Lot :</strong><br/>
                            ID Lot : <%: ViewBag.LotId %><br/>
                            ID Race : <%: ViewBag.RaceId %>
                        </div>

                        <!-- <% using (Html.BeginForm("MakaAtodySave", "DashBoard", FormMethod.Post)) { %> -->
                        <form action="/Lot/CreateLotAtody" method="post">
                            <%: Html.AntiForgeryToken() %>
                            <input type="hidden" name="lotId" value="<%: ViewBag.LotId %>" />
                            <input type="hidden" name="raceId" value="<%: ViewBag.RaceId %>" />

                            <div class="mb-3">
                                <label for="nomLot" class="form-label">Nouvel lot</label>
                                <input type="text" class="form-control" id="nomLot" name="nomLot" />
                            </div>

                            <div class="mb-3">
                                <label for="QuantiteAtody" class="form-label">Quantité d'Oeufs (Atody)</label>
                                <input type="number" class="form-control" id="quantiteAtody" name="quantiteAtody" min="1" />
                            </div>

                            <div class="d-grid mt-4">
                                <button type="submit" class="btn btn-success btn-lg">Enregistrer la Récolte</button>
                            </div>
                        </form>
                        <!-- <% } %> -->
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
