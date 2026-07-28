using AksharaMane.Application.Interfaces.Services;
using AksharaMane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Services
{
    public class OrderEmailTemplateService
    : IOrderEmailTemplateService
    {
        public string BuildOrderPlacedEmail(Order order)
        {
            var customerName =
                WebUtility.HtmlEncode(order.CustomerName);

            var bookTitle =
                WebUtility.HtmlEncode(order.Book.Title);

            var address =
                BuildAddress(order);

            return $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="UTF-8" />
        </head>
        <body style="
            margin:0;
            padding:0;
            background:#f4f1ea;
            font-family:Arial,Helvetica,sans-serif;
            color:#2f2f2f;">

            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" style="padding:30px 15px;">

                        <table width="600"
                               cellpadding="0"
                               cellspacing="0"
                               style="
                                  max-width:600px;
                                  background:#ffffff;
                                  border-radius:10px;
                                  overflow:hidden;">

                            <tr>
                                <td style="
                                    background:#5f4339;
                                    padding:24px;
                                    color:white;
                                    text-align:center;">

                                    <h1 style="margin:0;">
                                        AksharaMane
                                    </h1>

                                    <p style="margin:8px 0 0;">
                                        Your online bookstore
                                    </p>
                                </td>
                            </tr>

                            <tr>
                                <td style="padding:30px;">
                                    <h2 style="
                                        margin-top:0;
                                        color:#5f4339;">
                                        Order placed successfully
                                    </h2>

                                    <p>
                                        Hello {{customerName}},
                                    </p>

                                    <p>
                                        Thank you for placing your order.
                                        Your order has been received and
                                        will be processed shortly.
                                    </p>

                                    <table width="100%"
                                           cellpadding="10"
                                           cellspacing="0"
                                           style="
                                              border-collapse:collapse;
                                              margin-top:20px;">

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Order Number
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                <strong>
                                                    {{order.OrderNumber}}
                                                </strong>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Book
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                {{bookTitle}}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Quantity
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                {{order.Quantity}}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Total
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                ₹{{order.TotalAmount:N2}}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Payment
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                Cash on Delivery
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Status
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                {{order.Status}}
                                            </td>
                                        </tr>
                                    </table>

                                    <h3 style="
                                        margin-top:25px;
                                        color:#5f4339;">
                                        Delivery address
                                    </h3>

                                    <p style="line-height:1.7;">
                                        {{address}}
                                    </p>

                                    <p style="margin-top:30px;">
                                        Please keep your order number for
                                        tracking your order.
                                    </p>
                                </td>
                            </tr>

                            <tr>
                                <td style="
                                    background:#f7f5f0;
                                    padding:18px;
                                    text-align:center;
                                    font-size:13px;
                                    color:#666;">
                                    © AksharaMane
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>
        </body>
        </html>
        """;
        }

        public string BuildOrderStatusEmail(Order order)
        {
            var customerName =
                WebUtility.HtmlEncode(order.CustomerName);

            var status =
                WebUtility.HtmlEncode(order.Status.ToString());

            var bookTitle =
                WebUtility.HtmlEncode(order.Book.Title);

            return $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="UTF-8" />
        </head>
        <body style="
            margin:0;
            padding:0;
            background:#f4f1ea;
            font-family:Arial,Helvetica,sans-serif;
            color:#2f2f2f;">

            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" style="padding:30px 15px;">

                        <table width="600"
                               cellpadding="0"
                               cellspacing="0"
                               style="
                                  max-width:600px;
                                  background:#ffffff;
                                  border-radius:10px;
                                  overflow:hidden;">

                            <tr>
                                <td style="
                                    background:#5f4339;
                                    padding:24px;
                                    color:white;
                                    text-align:center;">

                                    <h1 style="margin:0;">
                                        AksharaMane
                                    </h1>
                                </td>
                            </tr>

                            <tr>
                                <td style="padding:30px;">
                                    <h2 style="
                                        margin-top:0;
                                        color:#5f4339;">
                                        Order status updated
                                    </h2>

                                    <p>
                                        Hello {{customerName}},
                                    </p>

                                    <p>
                                        The status of your order has been
                                        updated.
                                    </p>

                                    <table width="100%"
                                           cellpadding="10"
                                           cellspacing="0"
                                           style="
                                              border-collapse:collapse;
                                              margin-top:20px;">

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Order Number
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                {{order.OrderNumber}}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                Book
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                {{bookTitle}}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="
                                                background:#f7f5f0;
                                                border:1px solid #ddd;">
                                                New Status
                                            </td>

                                            <td style="
                                                border:1px solid #ddd;">
                                                <strong>
                                                    {{status}}
                                                </strong>
                                            </td>
                                        </tr>
                                    </table>

                                    {{BuildStatusMessage(order)}}
                                </td>
                            </tr>

                            <tr>
                                <td style="
                                    background:#f7f5f0;
                                    padding:18px;
                                    text-align:center;
                                    font-size:13px;
                                    color:#666;">
                                    © AksharaMane
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>
        </body>
        </html>
        """;
        }

        private static string BuildAddress(Order order)
        {
            var parts = new List<string>
        {
            WebUtility.HtmlEncode(order.AddressLine1)
        };

            if (!string.IsNullOrWhiteSpace(
                    order.AddressLine2))
            {
                parts.Add(
                    WebUtility.HtmlEncode(
                        order.AddressLine2));
            }

            parts.Add(
                WebUtility.HtmlEncode(order.City));

            parts.Add(
                WebUtility.HtmlEncode(order.State));

            parts.Add(
                WebUtility.HtmlEncode(order.Pincode));

            return string.Join("<br/>", parts);
        }

        private static string BuildStatusMessage(
            Order order)
        {
            var message = order.Status switch
            {
                Domain.Enums.OrderStatus.Confirmed =>
                    "Your order has been confirmed and will be prepared soon.",

                Domain.Enums.OrderStatus.Shipped =>
                    "Your order has been shipped and is on the way.",

                Domain.Enums.OrderStatus.Delivered =>
                    "Your order has been delivered. Thank you for shopping with us.",

                Domain.Enums.OrderStatus.Cancelled =>
                    "Your order has been cancelled.",

                _ =>
                    "Your order is being processed."
            };

            return $"""
        <p style="
            margin-top:25px;
            padding:15px;
            background:#f7f5f0;
            border-left:4px solid #5f4339;">
            {WebUtility.HtmlEncode(message)}
        </p>
        """;
        }
    }
}
