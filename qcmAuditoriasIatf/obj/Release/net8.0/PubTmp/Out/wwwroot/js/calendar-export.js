window.auditCalendarExport = {
    exportImage: async function (elementId, fileName) {
        const element = document.getElementById(elementId);
        if (!element) {
            throw new Error("No se encontró el contenedor a exportar.");
        }

        if (!window.html2canvas) {
            throw new Error("html2canvas no está cargado.");
        }

        const canvas = await window.html2canvas(element, {
            scale: 2,
            useCORS: true,
            backgroundColor: "#ffffff"
        });

        const link = document.createElement("a");
        link.download = `${fileName}.png`;
        link.href = canvas.toDataURL("image/png");
        link.click();
    },

    exportPdf: async function (elementId, fileName) {
        const element = document.getElementById(elementId);
        if (!element) {
            throw new Error("No se encontró el contenedor a exportar.");
        }

        if (!window.html2canvas) {
            throw new Error("html2canvas no está cargado.");
        }

        if (!window.jspdf) {
            throw new Error("jsPDF no está cargado.");
        }

        const canvas = await window.html2canvas(element, {
            scale: 2,
            useCORS: true,
            backgroundColor: "#ffffff"
        });

        const imgData = canvas.toDataURL("image/png");
        const { jsPDF } = window.jspdf;

        const pdf = new jsPDF("l", "mm", "a4");
        const pageWidth = pdf.internal.pageSize.getWidth();
        const pageHeight = pdf.internal.pageSize.getHeight();

        const margin = 5;
        const usableWidth = pageWidth - (margin * 2);
        const usableHeight = pageHeight - (margin * 2);

        const imgWidth = usableWidth;
        const imgHeight = (canvas.height * imgWidth) / canvas.width;

        let heightLeft = imgHeight;
        let position = margin;

        pdf.addImage(imgData, "PNG", margin, position, imgWidth, imgHeight);
        heightLeft -= usableHeight;

        while (heightLeft > 0) {
            position = heightLeft - imgHeight + margin;
            pdf.addPage();
            pdf.addImage(imgData, "PNG", margin, position, imgWidth, imgHeight);
            heightLeft -= usableHeight;
        }

        pdf.save(`${fileName}.pdf`);
    }
};