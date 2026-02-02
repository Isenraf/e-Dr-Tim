


  async function getServerTime() {
    const response = await fetch('/Ronde/ServerTime');
    const data = await response.json();
    return data.date; // retourne la date sous forme de string
  }

  async function getServerUser() {
    const response = await fetch('/Ronde/ServerUser');
    const data = await response.json();
    return data.utilisateur; // ⚠️ le backend doit renvoyer { user: "Nom Prénom" }
  }

  async function validerObservation(e, etat) {
    const button = e.target;
    const li = button.closest("li.list-group-item");
    if (!li) return;

    // Récupère le textarea lié au bouton
    const textarea = button.closest(".dropdown-menu")?.querySelector("textarea");
    const observation = textarea ? textarea.value.trim() : "";

    // Supprime les boutons du <li principal>
    li.querySelectorAll(".btn-group").forEach((b) => b.remove());

    // Récupère le texte de la prescription
    const prescriptionText = li.querySelector("span")?.innerHTML || "";

    // Vide le contenu du li
    li.innerHTML = "";

    // Récupère les infos du serveur
    const infirmierName = await getServerUser();
    const dateStr = await getServerTime();

    // Crée le wrapper
    const wrapperDiv = document.createElement("div");
    wrapperDiv.classList.add("d-flex", "justify-content-between", "align-items-center", "mb-1");

    const leftSpan = document.createElement("span");
    leftSpan.innerHTML = `<strong>${prescriptionText}</strong>`;

    const rightDiv = document.createElement("div");
    rightDiv.innerHTML = `
      <p>${infirmierName}</p>
      <small class="text-primary">${dateStr}</small>
    `;

    wrapperDiv.appendChild(leftSpan);
    wrapperDiv.appendChild(rightDiv);

    const obsDiv = document.createElement("div");
    obsDiv.classList.add("rounded", "obs-filled");
    obsDiv.textContent = observation;

    li.appendChild(wrapperDiv);
    li.appendChild(obsDiv);

    // Bordure couleur selon état
    li.style.borderLeft =
      etat === "fait" ? "4px solid #28a745" : "4px solid #dc3545";

    // Enregistrement côté serveur
    const taf = document.getElementById("taf").innerHTML;
    const nd = document.getElementById("identifiant").value;

    const donnees = {
      Taf: taf,
      Id: parseInt(nd)
    };

    console.log(donnees);

    $.ajax({
      type: "POST",
      contentType: "application/json; charset=utf-8",
      url: "/Ronde/Add2",
      dataType: "json",
      data: JSON.stringify(donnees),
      success: function (response) {
        console.log("Données enregistrées avec succès.");
      },
      error: function (response) {
        console.error("Erreur lors de l'enregistrement :", response);
        location.reload();
      }
    });
  }


