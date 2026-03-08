<%@ Page Language="C#" Inherits="AkohoAspx.ViewsCodeBehind.Home.HomeIndexPage" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Accueil - Akoho ASPX</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700;800&display=swap" rel="stylesheet">
    <style>
        :root {
            --primary-navy: #0f172a;
            --accent-blue: #3b82f6;
            --glass-bg: rgba(255, 255, 255, 0.75);
            --gradient-main: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
        }

        body {
            background: var(--gradient-main);
            background-image: 
                radial-gradient(at 0% 0%, rgba(59, 130, 246, 0.08) 0px, transparent 50%),
                radial-gradient(at 100% 100%, rgba(59, 130, 246, 0.05) 0px, transparent 50%);
            font-family: 'Inter', sans-serif;
            color: #1e293b;
            min-height: 100vh;
        }

        .hero-section {
            padding: 5rem 0 3rem;
            text-align: center;
        }

        .hero-title {
            font-weight: 800;
            font-size: 3.5rem;
            letter-spacing: -0.04em;
            background: linear-gradient(135deg, #0f172a 0%, #3b82f6 100%);
            -webkit-background-clip: text;
            background-clip: text;
            -webkit-text-fill-color: transparent;
            margin-bottom: 1rem;
        }

        .hero-subtitle {
            font-size: 1.25rem;
            color: #64748b;
            font-weight: 500;
            max-width: 600px;
            margin: 0 auto;
        }

        .glass-panel {
            background: var(--glass-bg);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border: 1px solid rgba(255, 255, 255, 0.5);
            border-radius: 32px;
            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.05);
            padding: 3rem;
            margin-bottom: 2rem;
        }

        .unit-card {
            background: white;
            border-radius: 20px;
            padding: 1.5rem;
            border: 1px solid #e2e8f0;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            height: 100%;
        }

        .unit-card:hover {
            transform: translateY(-8px);
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.05);
            border-color: var(--accent-blue);
        }

        .unit-icon {
            font-size: 2rem;
            margin-bottom: 1rem;
            display: inline-block;
        }

        .logic-step {
            position: relative;
            padding-left: 3rem;
            margin-bottom: 2.5rem;
        }

        .logic-step::before {
            content: "";
            position: absolute;
            left: 0.75rem;
            top: 2rem;
            bottom: -1rem;
            width: 2px;
            background: #e2e8f0;
        }

        .logic-step:last-child::before {
            display: none;
        }

        .step-number {
            position: absolute;
            left: 0;
            top: 0;
            width: 1.75rem;
            height: 1.75rem;
            background: var(--accent-blue);
            color: white;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: 800;
            font-size: 0.875rem;
            z-index: 2;
            box-shadow: 0 0 0 4px white;
        }

        .step-title {
            font-weight: 700;
            font-size: 1.125rem;
            margin-bottom: 0.5rem;
        }

        .step-desc {
            color: #64748b;
            line-height: 1.6;
        }

        .badge-unit {
            background: #eff6ff;
            color: #2563eb;
            font-weight: 700;
            padding: 0.2rem 0.6rem;
            border-radius: 6px;
        }
    </style>
</head>
<body>
    <% Html.RenderPartial("Navigation"); %>

    <div class="container py-4">
        <section class="hero-section">
            <h1 class="hero-title">Akoho</h1>
            <p class="hero-subtitle">Gestion du lot Akoho avec des oeufs.</p>
        </section>

        <div class="row g-4 mb-5">
            <div class="col-md-6">
                <div class="unit-card">
                    <span class="unit-icon">⚖️</span>
                    <h3 class="h5 fw-800">Unités de Poids</h3>
                    <p class="text-muted">Toutes les mesures de croissance et de consommation sont exprimées en <span class="badge-unit">grammes (g)</span>. Cela permet un suivi millimétré de l'évolution de vos poulets.</p>
                </div>
            </div>
            <div class="col-md-6">
                <div class="unit-card">
                    <span class="unit-icon">💰</span>
                    <h3 class="h5 fw-800">Unités Monétaires</h3>
                    <p class="text-muted">Les investissements, prix de vente et coûts de fonctionnement sont calculés en <span class="badge-unit">Ariary (Ar)</span>. Les prix unitaires sont dérivés dynamiquement du poids.</p>
                </div>
            </div>
        </div>

        <div class="glass-panel">
            <h2 class="h3 fw-800 mb-5 d-flex align-items-center gap-3">
                <span class="bg-primary text-white rounded-3 p-2 d-inline-flex" style="font-size: 1rem;">⚙️</span>
                Comment ça marche ?
            </h2>

            <div class="logic-step">
                <div class="step-number">1</div>
                <h3 class="step-title">Cycle de Consommation</h3>
                <p class="step-desc">Chaque semaine, le système consulte les tables de croissance de la race. La consommation est calculée en multipliant l'effectif actuel par le besoin hebdomadaire défini pour l'âge du lot.</p>
            </div>

            <div class="logic-step">
                <div class="step-number">2</div>
                <h3 class="step-title">Calcul des Dépenses de Nourriture</h3>
                <p class="step-desc">Il s'agit d'un cumul dynamique. Pour chaque semaine écoulée depuis la création du lot, le système applique le prix du sac de l'époque au volume mangé. <strong>Le montant affiché correspond au coût total investi en nourriture jusqu'à la date active.</strong></p>
            </div>

            <div class="logic-step">
                <div class="step-number">3</div>
                <h3 class="step-title">Gestion des Pertes (Décalage)</h3>
                <p class="step-desc">Lorsqu'une perte est signalée, elle affecte la consommation pour la semaine actuelle et les suivantes. Cela garantit que vous ne payez pas de nourriture pour des sujets qui ne sont plus dans l'effectif.</p>
            </div>

            <div class="logic-step">
                <div class="step-number">4</div>
                <h3 class="step-title">Estimation de Vente</h3>
                <p class="step-desc">Le système estime la valeur de votre lot en temps réel en multipliant le poids cumulé actuel par le prix au kilo de la race. Un badge apparaît automatiquement lorsque le lot atteint son poids optimal de vente.</p>
            </div>
        </div>
    </div>
</body>
</html>
