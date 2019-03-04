$(function() {
    $('.sidebar-menu').tree();

    document.getElementById("databases").addEventListener("change", function() {
        let x = document.getElementById("databases").value;
        let tablesList = ["Categories", "Customers", "Employees", "Orders", "Products"];
        let tables = document.getElementById("tables");

        while (tables.hasChildNodes()) {
            tables.removeChild(tables.firstChild);
        }

        for (let i = 0; i < tablesList.length; i++) {
            console.log(tablesList[i]);
            let element = document.createElement("option");
            element.value = tablesList[i];
            let text = document.createTextNode(tablesList[i]);
            element.appendChild(text);
            tables.appendChild(element);
        }
        tables.removeAttribute("disabled");

    });

    document.getElementById("columns").addEventListener("dblclick", function () {
        let x = document.getElementById("columns").value;
        alert(x);
    });

    sqlKeywords();
    sqlTextarea();


});


let dragObject;

function sqlKeywords() {
  let object = document.getElementById('sqlKeywords');
  let length = object.childNodes.length;

  for (let i = 0; i < length; i++) {
    object.childNodes[i].draggable = true;
    object.childNodes[i].addEventListener("dragstart", function(ev) {
      dragObject = ev.target.textContent;
      console.log(ev.target.textContent);
    });
  }
}

function sqlTextarea() {
  let object = document.getElementById('sqlTextarea');

  object.addEventListener("drop", function(ev) {
    ev.preventDefault();
    let text = object.textContent;
    text += dragObject + " ";
    object.textContent = text;
  });

  object.addEventListener("dragover", function(ev) {
    ev.preventDefault();
  });
}
