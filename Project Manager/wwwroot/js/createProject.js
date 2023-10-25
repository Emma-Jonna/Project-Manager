const materialUl = document.querySelector(".materials-list");
const addMaterialButton = document.querySelector(".add-material-button");

let materialIndex = 0;

addMaterialButton.addEventListener("click", () => {
    console.log("button clicked");

    CreateListItem();

    materialIndex++;
})

const CreateListItem = () => {

    const materialContainer = document.createElement("li");
    materialContainer.id = materialIndex;

    const materialNameInput = document.createElement("input");
    const materialAmountInput = document.createElement("input");
    const materialAcquiredInput = document.createElement("input");

    materialNameInput.type = "text";
    materialNameInput.name = `Material[${materialIndex}].Name`;

    materialAmountInput.type = "text";
    materialAmountInput.name = `Material[${materialIndex}].Amount`;

    materialAcquiredInput.type = "checkbox";
    materialAcquiredInput.name = `Material[${materialIndex}].Acquired`;

    materialContainer.appendChild(materialNameInput);
    materialContainer.appendChild(materialAmountInput);
    materialContainer.appendChild(materialAcquiredInput);
    materialUl.appendChild(materialContainer);
    
} 

