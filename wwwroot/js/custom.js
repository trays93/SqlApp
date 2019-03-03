$(function() {
  $('.sidebar-menu').tree();

  databaseKeywords();
  sqlKeywords();
  sqlTextarea();

});


let dragObject;

function databaseKeywords() {
  let object = document.getElementById('databaseNames');
  let length = object.childNodes.length;

  for (let i = 0; i < length; i++) {
    object.childNodes[i].draggable = true;
    object.childNodes[i].addEventListener("dragstart", function(ev) {
      let text = ev.target.textContent;
      text = text.trim();
      dragObject = "[" + text + "]";
      console.log(dragObject);
    });
  }

  object = document.getElementsByClassName('tableNames');
  length = object.length;

  for (let i = 0; i < length; i++) {
    let childNodesLength = object[i];
    for (let j = 0; j < childNodesLength; j++) {
      object[i].childNodes[j].draggable = true;
      object[i].childNodes[j].addEventListener("dragstart", function(ev) {
        let text = ev.target.textContent;
        text = text.trim();
        dragObject = "[" + text + "]" + ", ";
        console.log(dragObject);
      });
    }
  }

}

function sqlKeywords() {
  let object = document.getElementById('sqlKeywords2');
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
