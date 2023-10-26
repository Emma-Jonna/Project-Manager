const materialUl = document.querySelector("ul");
const addMaterialButton = document.querySelector(".add-material-button");
const listItems = document.querySelectorAll(".materials-list li");
const checkboxes = document.querySelectorAll("li [type=checkbox]");

let materialIndex = 0;
let idIndex = 0;

addMaterialButton.addEventListener("click", () => {
    //console.log("button clicked");

    CreateListItem();

    idIndex++;
});

const DeleteMaterial = (event) => {
    //console.log(event.target.id);    
    var button = document.getElementById(event.id);

    //console.log(materialUl);

    for (var i = 0; i < materialUl.children.length; i++) {
        //console.log(i);
        //console.log(materialUl.children[i].id);
        //console.log(event.target.id);


        if (materialUl.children[i].id == `list-${event.target.id}`) {
            //console.log("match");
            var listItem = document.getElementById(`list-${event.target.id}`);
            //console.log(listItem);

            materialUl.removeChild(listItem);

            /*if (materialUl.length == 1) {
                materialUl.removeChild(materialUl.firstChild);
            }*/
        } else {
            //console.log("no match");
        }
    }
    //console.log(listItem);


    // all list items in an array
    let array = [...materialUl.children];

    //console.log(array);

    // printing all list items inputs
    //idIndex = 0;

    array.forEach((element) => {


        //console.log(element.id);
        //element.id = `list-${idIndex}`;
        //idIndex++;

        for (let i = 0; i < element.children.length; i++) {
            //console.log(element.children[i]);
            //console.log(element.children[i].name);            


            //element.children[i].name = `Material[${materialIndex}].Amount`;
        }
    });
}

const CheckCheckBox = (event) => {
    if (event.target.checked) {
        //console.log("checked");
        event.target.value = true;
    } else {
        //console.log("not checked");
        event.target.value = false;
    }
}

const CreateListItem = () => {

    const materialContainer = document.createElement("li");
    materialContainer.id = `list-${idIndex}`;
    materialContainer.classList.add("material-container");

    const materialNameInput = document.createElement("input");
    const materialAmountInput = document.createElement("input");
    const materialAcquiredInput = document.createElement("input");
    const deleteMaterialInput = document.createElement("div");
    const deleteMaterialTextbox = document.createElement("p");

    materialNameInput.type = "text";
    materialNameInput.name = `Material[${materialIndex}].Name`;

    materialAmountInput.type = "text";
    materialAmountInput.name = `Material[${materialIndex}].Amount`;

    materialAcquiredInput.type = "checkbox";
    materialAcquiredInput.name = `Material[${materialIndex}].Acquired`;
    materialAcquiredInput.value = false;
    materialAcquiredInput.addEventListener('change', CheckCheckBox);

    deleteMaterialInput.name = `Material[${materialIndex}].Acquired`;
    deleteMaterialInput.id = `${idIndex}`
    deleteMaterialInput.addEventListener('click', DeleteMaterial);

    deleteMaterialTextbox.textContent = "Delete";
    deleteMaterialTextbox.id = `${idIndex}`

    materialContainer.appendChild(materialNameInput);
    materialContainer.appendChild(materialAmountInput);
    materialContainer.appendChild(materialAcquiredInput);
    materialContainer.appendChild(deleteMaterialInput);
    deleteMaterialInput.appendChild(deleteMaterialTextbox);

    materialUl.appendChild(materialContainer);
}

