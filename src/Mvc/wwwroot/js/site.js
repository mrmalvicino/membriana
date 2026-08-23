document.querySelectorAll("[data-image-url-editor]").forEach((editor) => {
	const input = editor.querySelector("[data-image-url-input]");
	const preview = editor.querySelector("[data-image-preview]");
	const placeholder = editor.querySelector("[data-image-placeholder]");

	if (!input || !preview || !placeholder) {
		return;
	}

	const showPlaceholder = () => {
		preview.hidden = true;
		placeholder.hidden = false;
	};

	const updatePreview = () => {
		const url = input.value.trim();

		if (!url) {
			preview.removeAttribute("src");
			placeholder.textContent = "Sin imagen";
			showPlaceholder();
			return;
		}

		if (!input.checkValidity()) {
			preview.removeAttribute("src");
			placeholder.textContent = "URL no válida";
			showPlaceholder();
			return;
		}

		preview.src = url;
		preview.hidden = false;
		placeholder.hidden = true;
	};

	preview.addEventListener("error", () => {
		placeholder.textContent = "No disponible";
		showPlaceholder();
	});

	preview.addEventListener("load", () => {
		preview.hidden = false;
		placeholder.hidden = true;
	});

	input.addEventListener("input", updatePreview);
	updatePreview();
});
