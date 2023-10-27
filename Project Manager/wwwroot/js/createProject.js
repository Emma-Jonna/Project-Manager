const materialUl = document.querySelector("ul");
const addMaterialButton = document.querySelector(".add-material-button");
const listItems = document.querySelectorAll(".materials-list li");
const checkboxes = document.querySelectorAll("li [type=checkbox]");
const defaultButton = document.querySelectorAll(".default-list-item-button");
//const defaultButton = document.getElementsByClassName(".default-list-item-button");

let materialIndex = 0;

addMaterialButton.addEventListener("click", () => {
    
    CreateListItem();

    materialIndex++;
});

const DeleteMaterial = (event) => {
    console.log(event);
    ChangeListItemsIds(event);
}

const ChangeListItemsIds = (event) => {

    for (var i = 0; i < materialUl.children.length; i++) {

        if (materialUl.children[i].id == `list-${event.target.id}`) {

            var listItem = document.getElementById(`list-${event.target.id}`);

            materialUl.removeChild(listItem);
        }
    }

    let array = [...materialUl.children];
    materialIndex = 0;

    ChangeElementsIds(array);
}

const ChangeElementsIds = (array) => {

    array.forEach((element) => {

        element.id = `list-${materialIndex}`;

        for (let i = 0; i < element.children.length; i++) {

            let inputClassName = element.children[i].classList.value;

            if (inputClassName == "material-name") {

                element.children[i].name = `Material[${materialIndex}].Name`;

            } else if (inputClassName == "material-amount") {

                element.children[i].name = `Material[${materialIndex}].Amount`;

            } else if (inputClassName == "material-checkbox") {

                element.children[i].name = `Material[${materialIndex}].Acquired`;

            } else if (inputClassName == "delete-button-container") {

                element.children[i].id = materialIndex;

                element.children[i].children[0].id = materialIndex;
            }
        }

        materialIndex++;
    });
}

const CheckCheckBox = (event) => {
    if (event.target.checked) {        
        event.target.value = true;
    } else {        
        event.target.value = false;
    }
}

const CreateListItem = () => {

    const materialContainer = document.createElement("li");
    materialContainer.id = `list-${materialIndex}`;
    materialContainer.classList.add("material-container");

    const materialNameInput = document.createElement("input");
    const materialAmountInput = document.createElement("input");
    const materialAcquiredInput = document.createElement("input");
    const deleteMaterialInput = document.createElement("div");
    const deleteMaterialTextbox = document.createElement("p");

    materialNameInput.type = "text";
    materialNameInput.name = `Material[${materialIndex}].Name`;
    materialNameInput.classList.add("material-name");

    materialAmountInput.type = "text";
    materialAmountInput.name = `Material[${materialIndex}].Amount`;
    materialAmountInput.classList.add("material-amount");

    materialAcquiredInput.type = "checkbox";
    materialAcquiredInput.name = `Material[${materialIndex}].Acquired`;
    materialAcquiredInput.value = false;
    materialAcquiredInput.addEventListener('change', CheckCheckBox);
    materialAcquiredInput.classList.add("material-checkbox");

    deleteMaterialInput.name = `Material[${materialIndex}].Acquired`;
    deleteMaterialInput.id = `${materialIndex}`
    deleteMaterialInput.addEventListener('click', DeleteMaterial);
    deleteMaterialInput.classList.add("delete-button-container");

    deleteMaterialTextbox.textContent = "Delete";
    deleteMaterialTextbox.id = `${materialIndex}`

    materialContainer.appendChild(materialNameInput);
    materialContainer.appendChild(materialAmountInput);
    materialContainer.appendChild(materialAcquiredInput);
    materialContainer.appendChild(deleteMaterialInput);
    deleteMaterialInput.appendChild(deleteMaterialTextbox);

    materialUl.appendChild(materialContainer);
}

let array = [...materialUl.children];

ChangeElementsIds(array);


defaultButton.forEach((button) => button.addEventListener('click', DeleteMaterial));
checkboxes.forEach((checkbox) => checkbox.addEventListener('click', CheckCheckBox));

console.log(defaultButton);
console.log(array);