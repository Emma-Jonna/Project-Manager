const materialUl = document.querySelector(".materials-list");
const addMaterialButton = document.querySelector(".add-material-button");
const checkboxes = document.querySelectorAll("li [type=checkbox]");

let materialIndex = 0;

addMaterialButton.addEventListener("click", () => {
    console.log("button clicked");

    CreateListItem();

    materialIndex++;
});

const CheckCheckBox = (event) => {
    if (event.target.checked) {
        console.log("checked");
        event.target.value = true;
    } else {
        console.log("not checked");
        event.target.value = false;
    }
}

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
    materialAcquiredInput.value = false;
    materialAcquiredInput.addEventListener('change', CheckCheckBox);

    materialContainer.appendChild(materialNameInput);
    materialContainer.appendChild(materialAmountInput);
    materialContainer.appendChild(materialAcquiredInput);
    materialUl.appendChild(materialContainer);
} 

