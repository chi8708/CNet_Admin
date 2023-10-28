<template>
  <div>
    <Tree :data="SortTree"  @on-select-change="sortTreeChange" class="demo-tree-render"></Tree>
  </div>
</template>
<script>
import { getSorts } from "@/api/wikiSort";
import { resolveComponent } from 'vue'
export default {
  props: {
    parent: {}
  },
  data() {
    return {
      Sorts: [],
      SortTree: [],
      SortTreeItems: [],
      buttonProps: {
        type: 'default',
        size: 'small',
      }
    };
  },
  methods: {
    getSorts() {
      this.Sorts = [];
      this.SortTree = [];
      this.SortTreeItems = [];
      getSorts()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.Sorts = data;
          this.loadSortTree();
        })
        .catch(err => { });
    },
    loadSortTree() {
      var treeRoot = this.Sorts.filter(p => p.parentCode == "");
      this.loadSortTreeChild(treeRoot);
    },
    loadSortTreeChild(treeData) {
      for (let index = 0; index < treeData.length; index++) {
        const element = treeData[index];
        this.loadSortTreeItem(element);
        const child = this.Sorts.filter(p => p.parentCode == element.sortCode);
        if (child && child.length > 0) {
          this.loadSortTreeChild(child);
        }
      }
    },
    loadSortTreeItem(treeItemData) {
      var treeItem = {
        title: treeItemData.sortName,
        expand: true,
        value: treeItemData.sortCode,
      };
      this.SortTreeItems.push(treeItem);
      if (treeItemData.parentCode) {
        //foreach 无法终止循环
        this.SortTreeItems.every(element => {
          if (element.value == treeItemData.parentCode) {
            if (!element.children) {
              element.children = [];
            }
            element.children.push(treeItem);
            return false;
          }
          return true;
        });

      }
      //跟节点
      else {
        this.SortTree.push(treeItem);
      }
    },
    sortSelect() {
      if (!this.Sorts || this.Sorts.length <= 0) {
        this.getSorts();
      }
    },
    sortTreeChange(data) {
      var item0 = data[0];
      if (!item0) {
        this.parent.modelSort = false;
        return;
      }
      this.parent.sortChange(item0.value, item0.title);
    }
  },
  mounted() {
    this.getSorts();
  }
};
</script>