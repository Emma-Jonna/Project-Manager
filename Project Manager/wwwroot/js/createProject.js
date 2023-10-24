const materialUl = document.querySelector(".materials-list");
const materialName = document.createElement("p");
const materialItem = document.createElement("li");


materialItem.setAttribute("name", 8);
materialUl.appendChild(materialItem);


materialName.textContent = "Fabric";
materialItem.appendChild(materialName);


const materialList = [];