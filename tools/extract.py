import os
import zipfile
import shutil
import re

# Define the base folder names for each OS
base_folders = {
    'osx': 'osx-{arch}',          # macOS (now osx)
    'windows': 'win-{arch}',      # Windows
    'ubuntu': 'linux-{arch}',     # Ubuntu
    'android': 'android-{arch}',   # Android folders
}

def extract_zip_files():
    # Get the list of all zip files in the current directory
    zip_files = [f for f in os.listdir() if f.endswith('.zip')]
    
    # Extract each zip file into its respective folder
    for zip_file in zip_files:
        try:
            # Determine the OS and architecture from the filename
            match = re.match(r'(\w*)-(\w*)-latest-(\w*)-artifacts\.zip', zip_file)
            if match:
                lib_name, os_name, arch = match.groups()

                # Map OS names and architecture formats
                if os_name == 'macos':
                    os_name = 'osx'
                if arch == 'x86_64':
                    arch = 'x64'

                # Create target folder using actual architecture
                target_folder = base_folders[os_name].format(arch=arch)
                os.makedirs(target_folder, exist_ok=True)

                # Extract the zip file
                with zipfile.ZipFile(zip_file, 'r') as zip_ref:
                    zip_ref.extractall(target_folder)
                    print(f"Extracted {zip_file} into {target_folder}/")

                # Handle special cases for Android binaries
                if os_name == 'android':
                    # Move contents from arm64-v8a and x86_64 to their respective folders
                    for arch_folder in ['arm64-v8a', 'x86_64']:
                        arch_path = os.path.join(target_folder, arch_folder)
                        if os.path.exists(arch_path):
                            new_arch_folder = base_folders['android'].format(arch=arch_folder.replace('arm64-v8a', 'arm64').replace('x86_64', 'x64'))
                            os.makedirs(new_arch_folder, exist_ok=True)
                            # Move all files from the arch_path to the new_arch_folder
                            for item in os.listdir(arch_path):
                                source_file = os.path.join(arch_path, item)
                                destination_file = os.path.join(new_arch_folder, item)
                                shutil.move(source_file, destination_file)
                                print(f"Moved {item} from {arch_path}/ to {new_arch_folder}/")
                            # Optionally delete the now-empty directory
                            shutil.rmtree(arch_path)
                            print(f"Deleted the directory at {arch_path}/")
                    
                    # Clean up the android-all orphan folder if it exists
                    if os.path.exists(target_folder):
                        shutil.rmtree(target_folder)
                        print(f"Deleted the orphan directory at {target_folder}/")

            else:
                print(f"Unknown file format for {zip_file}. Skipping...")

        except Exception as e:
            print(f"Failed to extract {zip_file}: {e}")

if __name__ == "__main__":
    extract_zip_files()
