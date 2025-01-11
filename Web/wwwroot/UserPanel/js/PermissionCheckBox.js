document.addEventListener("DOMContentLoaded", function () {
    const permissionCheckboxes = document.querySelectorAll(".permission-checkbox");

    // Add event listeners to all checkboxes
    permissionCheckboxes.forEach(checkbox => {
        checkbox.addEventListener("change", function () {
            handleCheckboxChange(checkbox);
        });
    });

    function handleCheckboxChange(checkbox) {
        const permissionId = checkbox.dataset.permissionId;
        const isParent = !checkbox.dataset.parentId; // Parent has no parentId

        if (isParent) {
            // If parent checkbox is changed, update all child checkboxes
            const childCheckboxes = document.querySelectorAll(`[data-parent-id="${permissionId}"]`);
            childCheckboxes.forEach(childCheckbox => {
                if (childCheckbox.checked !== checkbox.checked) {
                    childCheckbox.checked = checkbox.checked;
                    childCheckbox.dispatchEvent(new Event("change", {bubbles: true}));
                }
            });
        } else {
            // If child checkbox is changed, update parent checkbox
            const parentId = checkbox.dataset.parentId;
            const parentCheckbox = document.querySelector(`[data-permission-id="${parentId}"]`);

            if (parentCheckbox) {
                const childCheckboxes = document.querySelectorAll(`[data-parent-id="${parentId}"]`);
                const atLeastOneChildChecked = Array.from(childCheckboxes).some(child => child.checked);

                // Update parent checkbox based on child checkboxes
                parentCheckbox.checked = atLeastOneChildChecked;
                parentCheckbox.disabled = !atLeastOneChildChecked; // Disable parent if all children are off
            }
        }
    }

    // Initialize parent checkboxes based on child states
    initializeParentCheckboxes();

    function initializeParentCheckboxes() {
        const parentCheckboxes = document.querySelectorAll(".permission-checkbox[data-parent-id='']");
        parentCheckboxes.forEach(parentCheckbox => {
            const permissionId = parentCheckbox.dataset.permissionId;
            const childCheckboxes = document.querySelectorAll(`[data-parent-id="${permissionId}"]`);
            const atLeastOneChildChecked = Array.from(childCheckboxes).some(child => child.checked);

            // Set initial state of parent checkbox
            parentCheckbox.checked = atLeastOneChildChecked;
            parentCheckbox.disabled = !atLeastOneChildChecked; // Disable parent if all children are off
        });
    }
});