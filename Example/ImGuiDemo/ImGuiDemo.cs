namespace Example.ImGuiDemo
{
    using HexaEngine.ImGuiNET;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImGuiDemo
    {
        private ImGuiTreeNodeFlags base_flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | ImGuiTreeNodeFlags.SpanAvailWidth;
        private bool align_label_with_current_x_position = false;
        private bool test_drag_and_drop = false;

        private int selection_mask = (1 << 2);

        private void HelpMarker(string desc)
        {
            ImGui.TextDisabled("(?)");
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.DelayShort) && ImGui.BeginTooltip())
            {
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted(desc);
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
        }

        public unsafe void Draw()
        {
            if (!ImGui.Begin("Demo ImGui"))
            {
                ImGui.End();
                return;
            }
            if (ImGui.TreeNode("Basic"))
            {
                // Here we will showcase three different ways to output a table.
                // They are very simple variations of a same thing!

                // [Method 1] Using TableNextRow() to create a new row, and TableSetColumnIndex() to select the column.
                // In many situations, this is the most flexible and easy to use pattern.
                HelpMarker("Using TableNextRow() + calling TableSetColumnIndex() _before_ each cell, in a loop.");
                if (ImGui.BeginTable("table1", 3))
                {
                    for (int row = 0; row < 4; row++)
                    {
                        ImGui.TableNextRow();
                        for (int column = 0; column < 3; column++)
                        {
                            ImGui.TableSetColumnIndex(column);
                            ImGui.Text($"Row {row} Column {column}");
                        }
                    }
                    ImGui.EndTable();
                }

                // [Method 2] Using TableNextColumn() called multiple times, instead of using a for loop + TableSetColumnIndex().
                // This is generally more convenient when you have code manually submitting the contents of each column.
                HelpMarker("Using TableNextRow() + calling TableNextColumn() _before_ each cell, manually.");
                if (ImGui.BeginTable("table2", 3))
                {
                    for (int row = 0; row < 4; row++)
                    {
                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();
                        ImGui.Text($"Row {row}");
                        ImGui.TableNextColumn();
                        ImGui.Text("Some contents");
                        ImGui.TableNextColumn();
                        ImGui.Text("123.456");
                    }
                    ImGui.EndTable();
                }

                // [Method 3] We call TableNextColumn() _before_ each cell. We never call TableNextRow(),
                // as TableNextColumn() will automatically wrap around and create new rows as needed.
                // This is generally more convenient when your cells all contains the same type of data.
                HelpMarker(
                    "Only using TableNextColumn(), which tends to be convenient for tables where every cell contains the same type of contents.\n"

                    + "This is also more similar to the old NextColumn() function of the Columns API, and provided to facilitate the Columns->Tables API transition.");
                if (ImGui.BeginTable("table3", 3))
                {
                    for (int item = 0; item < 14; item++)
                    {
                        ImGui.TableNextColumn();
                        ImGui.Text($"Item {item}");
                    }
                    ImGui.EndTable();
                }

                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Basic trees"))
            {
                for (int i = 0; i < 5; i++)
                {
                    // Use SetNextItemOpen() so set the default state of a node to be open. We could
                    // also use TreeNodeEx() with the ImGuiTreeNodeFlags.DefaultOpen flag to achieve the same thing!
                    if (i == 0)
                        ImGui.SetNextItemOpen(true, ImGuiCond.Once);

                    if (ImGui.TreeNode($"{i}", $"Child {i}"))
                    {
                        ImGui.Text("blah blah");
                        ImGui.SameLine();
                        if (ImGui.SmallButton("button")) { }
                        ImGui.TreePop();
                    }
                }
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Advanced, with Selectable nodes"))
            {
                HelpMarker(
                    "This is a more typical looking tree with selectable nodes.\n"

                   + "Click to select, CTRL+Click to toggle, click on arrows or double-click to open.");

                var iflags = (int)base_flags;
                ImGui.CheckboxFlags("ImGuiTreeNodeFlags.OpenOnArrow", ref iflags, (int)ImGuiTreeNodeFlags.OpenOnArrow);
                ImGui.CheckboxFlags("ImGuiTreeNodeFlags.OpenOnDoubleClick", ref iflags, (int)ImGuiTreeNodeFlags.OpenOnDoubleClick);
                ImGui.CheckboxFlags("ImGuiTreeNodeFlags.SpanAvailWidth", ref iflags, (int)ImGuiTreeNodeFlags.SpanAvailWidth); ImGui.SameLine(); HelpMarker("Extend hit area to all available width instead of allowing more items to be laid out after the node.");
                ImGui.CheckboxFlags("ImGuiTreeNodeFlags.SpanFullWidth", ref iflags, (int)ImGuiTreeNodeFlags.SpanFullWidth);
                base_flags = (ImGuiTreeNodeFlags)iflags;

                ImGui.Checkbox("Align label with current X position", ref align_label_with_current_x_position);
                ImGui.Checkbox("Test tree node as drag source", ref test_drag_and_drop);
                ImGui.Text("Hello!");
                if (align_label_with_current_x_position)
                    ImGui.Unindent(ImGui.GetTreeNodeToLabelSpacing());

                // 'selection_mask' is dumb representation of what may be user-side selection state.
                //  You may retain selection state inside or outside your objects in whatever format you see fit.
                // 'node_clicked' is temporary storage of what node we have clicked to process selection at the end
                /// of the loop. May be a pointer to your own node type, etc.

                int node_clicked = -1;
                for (int i = 0; i < 6; i++)
                {
                    // Disable the default "open on single-click behavior" + set Selected flag according to our selection.
                    // To alter selection we use IsItemClicked() && !IsItemToggledOpen(), so clicking on an arrow doesn't alter selection.
                    ImGuiTreeNodeFlags node_flags = base_flags;
                    bool is_selected = (selection_mask & (1 << i)) != 0;
                    if (is_selected)
                        node_flags |= ImGuiTreeNodeFlags.Selected;
                    if (i < 3)
                    {
                        // Items 0..2 are Tree Node
                        bool node_open = ImGui.TreeNodeEx($"i", node_flags, $"Selectable Node {i}");
                        if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
                            node_clicked = i;
                        if (test_drag_and_drop && ImGui.BeginDragDropSource())
                        {
                            ImGui.SetDragDropPayload("_TREENODE", null, 0);
                            ImGui.Text("This is a drag and drop source");
                            ImGui.EndDragDropSource();
                        }
                        if (node_open)
                        {
                            ImGui.BulletText("Blah blah\nBlah Blah");
                            ImGui.TreePop();
                        }
                    }
                    else
                    {
                        // Items 3..5 are Tree Leaves
                        // The only reason we use TreeNode at all is to allow selection of the leaf. Otherwise we can
                        // use BulletText() or advance the cursor by GetTreeNodeToLabelSpacing() and call Text().
                        node_flags |= ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen; // ImGuiTreeNodeFlags.Bullet
                        ImGui.TreeNodeEx($"i", node_flags, $"Selectable Leaf {i}");
                        if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
                            node_clicked = i;
                        if (test_drag_and_drop && ImGui.BeginDragDropSource())
                        {
                            ImGui.SetDragDropPayload("_TREENODE", null, 0);
                            ImGui.Text("This is a drag and drop source");
                            ImGui.EndDragDropSource();
                        }
                    }
                }
                if (node_clicked != -1)
                {
                    // Update selection state
                    // (process outside of tree loop to avoid visual inconsistencies during the clicking frame)
                    if (ImGui.GetIO().KeyCtrl)
                        selection_mask ^= (1 << node_clicked);          // CTRL+click to toggle
                    else //if (!(selection_mask & (1 << node_clicked))) // Depending on selection behavior you want, may want to preserve selection when clicking on item that is part of the selection
                        selection_mask = (1 << node_clicked);           // Click to single-select
                }
                if (align_label_with_current_x_position)
                    ImGui.Indent(ImGui.GetTreeNodeToLabelSpacing());
                ImGui.TreePop();
            }
            ImGui.TreePop();

            ImGui.End();
        }
    }
}