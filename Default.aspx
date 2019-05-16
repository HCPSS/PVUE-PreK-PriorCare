<%@ Page Title="Prior Care" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">
        <h2><%: Title %> </h2>
        <h3>
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </h3>
        <div role="alert" class="alert alert-warning" id="ErrorDiv" runat="server">
            <asp:Label runat="server" ID="ErrorMsg"></asp:Label>
        </div>
        <div runat="server" id="divStudentInfo">
            <div class="panel panel-default">
                <div class="panel-body">
                    <asp:Table ID="tblStudentInfo" runat="server" CellPadding="1" CellSpacing="10" Width="100%" BorderWidth="1" HorizontalAlign="Left">
                        <asp:TableRow>
                            <asp:TableCell Width="40%">
                                <strong>Student Name: </strong>
                                <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <strong>Date of Birth: </strong>
                                <asp:Label ID="lblDOB" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow>
                            <asp:TableCell Width="25%">
                                <strong>School: </strong>
                                <asp:Label ID="lblCurrentSchool" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow>
                            <asp:TableCell>
                                <strong>Grade: </strong>
                                <asp:Label ID="lblGrade" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>
                        
                        <asp:TableRow>
                            <asp:TableCell>
                                <strong>Student ID: </strong>
                                <asp:Label ID="lblsisNumber" runat="server"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell></asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell Width="50%">
                                 <asp:DropDownList ID="DDLTimeSpent" runat="server" AutoPostBack="true" 
                                        DataValueField="value_code" DataTextField="value_description" 
                                        OnSelectedIndexChanged="DDLTimeSpent_SelectedIndexChanged"
                                        visible="false">
                                </asp:DropDownList>           
                            </asp:TableCell>
                            <asp:TableCell Width="50%">
                                <asp:DropDownList ID="DDLPriorCare" runat="server" AutoPostBack="true" 
                                        DataValueField="value_code" DataTextField="value_description" 
                                        onSelectedIndexChanged="DDLPriorCare_SelectedIndexChanged"
                                        visible="false">
                                </asp:DropDownList>
                            </asp:TableCell>
                       </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Image ID="FullDayImage" runat="server" visible="true" Enabled="true" ImageUrl="images/checkmark.png" AlternateText="No Prior Care Action Needed" ImageAlign="Left"/>
                                <asp:Label ID="LBLUserMessage" runat="server" Visible="true" Enabled="true" Text="No prior care action needed."></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btnInsert" runat="Server" OnClick="btnInsert_Click1" Text="Add New Prior Care Row" Visible="true"/>
                                <asp:Button ID="btnSave" runat="Server" OnClick="btnSave_Click" Text="Save New Row" Visible="false"/>
                                <asp:Button ID="btnDiscard" runat="Server" OnClick="btnDiscard_Click" Text="Discard Changes" Visible="false"/>
                            </asp:TableCell>
                        </asp:TableRow>

                       </asp:Table>
                       <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="10" Width="100%" BorderWidth="0" HorizontalAlign="Left">
                       
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:GridView ID="gvPriorCare" DataKeyNames="rowguid" runat="server" AutoGenerateColumns="false" CellPadding="50" CellSpacing="50" Visible="true" Enabled="true" OnRowDeleting="gvPriorCare_RowDeleting" onDataLoad="" GridLines="None">
                                    <Columns>
                                        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" />
                                        
                                        <asp:TemplateField>
                                            <HeaderStyle Font-Bold="True" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Width="100%"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="HDNrowguid" runat="server" Value='<%# Eval("rowguid")%>' />
                                                <asp:HiddenField ID="HDNTimeSpentCode" runat="server" Value='<%# Eval("TIME_SPENT_CODE")%>' />
                                                <asp:HiddenField ID="HDNPriorCareCode" runat="server" Value='<%# Eval("PRIOR_CARE_CODE")%>' />
                                                <asp:Label ID="TBTimeSpent" runat="server" AutoPostBack="false" Enabled="false" Visible="true" Text='<%# Eval("TIME_SPENT")+"-"+Eval("PRIOR_CARE")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

<%--                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="ButtonEdit" runat="Server" OnClick="RowEditing" Text="Edit Row"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="ButtonDelete" runat="Server" OnClick="ButtonDelete_Click" Text="Delete Row"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                    </Columns>
                                </asp:GridView>

                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
 
                </div>
            </div>
        </div>
    </form>
</asp:Content>

