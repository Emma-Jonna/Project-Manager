const materialUl = document.querySelector(".materials-list");
const addMaterialButton = document.querySelector(".add-material-button");
const listItems = document.querySelectorAll(".materials-list li");
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
    materialContainer.classList.add("material-container");

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

    // all list items in an array
    let array = [...materialUl.children];
    let childrenArray = [];

    console.log(array);

    // printing all list items inputs

    array.forEach((element) => {
        for (let i = 0; i < element.children.length; i++) {
            console.log(element.children[i]);
            console.log(element.children[i].name);
            //element.children.setAttribute("Name", `Material[8].Amount`);
            element.children[i].name = "Material[8].Amount";
            //("ul > li").classList.add(("Material[8].Amount"));
        }
    });
}

