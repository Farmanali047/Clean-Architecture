var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start();
connection.on("ReceiveMessage", function (productNames) {
    console.log("yes");
  //  alert(productNames);
    Toastify({
        text: `
            <div style="color: black; font-family: 'BloodNET-Regular', Arial, sans-serif; padding: 15px; border-radius: 8px; border: 1px solid #ddd; background-color: #f9f9f9;">
                <span style="font-size: 16px; color: #555;"><strong>💉 ${productNames}</strong>  Has Booked Now </span><br><br>
            </div>
        `,
        duration: 8000,
        gravity: "bottom",
        position: "right",
        backgroundColor: "transparent", // Ensure the Toastify background is transparent
        stopOnFocus: true,
        escapeMarkup: false,
        className: "custom-toast" // Add a custom class for additional styling
    }).showToast();
});
